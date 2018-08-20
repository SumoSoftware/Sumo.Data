using Sumo.Data.Orm;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public static class RecordsetExtensions
    {
        public static T ToObject<T>(this Recordset recordset, long recordIndex) where T : class
        {
            // allows activator to use non-public constructors
            var result = (T)Activator.CreateInstance(
                typeof(T),
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                null, null, null);

            for (var i = 0; i < TypeInfoCache<T>.ReadWriteProperties.Length; ++i)
            {
                var property = TypeInfoCache<T>.ReadWriteProperties[i];
                var propertyName = TypeInfoCache<T>.ReadWritePropertyNames[i];
                var value = recordset[recordIndex, propertyName];
                if (value != null)
                {
                    //todo: this can be optimized by passing in the table definition or a cache of row types from the ToArray methods
                    //todo: this can be optimized by getting the underlying nullable types in TypeInfoCache 
                    var propertyType = property.PropertyType.IsNullable() ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;
                    value = propertyType == value.GetType() ? value : Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(result, value);
                }
            }
            return result;
        }

        /// <summary>
        /// for binary types
        /// </summary>
        /// <param name="recordset"></param>
        /// <returns></returns>
        public static object[] ToObjectArray(this Recordset recordset)
        {
            var result = new object[recordset.Count];
            for (var i = 0; i < recordset.Count; ++i)
            {
                result[i] = recordset[i, 0];
            }
            return result;
        }

        /// <summary>
        /// for value/scalor types like string, datetime, and numeric types 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recordset"></param>
        /// <returns></returns>
        public static T[] ToValueArray<T>(this Recordset recordset) where T : struct
        {
            var result = new T[recordset.Count];
            if (recordset.Count > 0)
            {
                var type = typeof(T);
                type = type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
                var doConvert = type != recordset[0, 0].GetType();
                for (var i = 0; i < recordset.Count; ++i)
                {
                    result[i] = (T)(doConvert ? Convert.ChangeType(recordset[i][0], type) : recordset[i][0]);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplentation">The type to use to for the interface implementation. Must have public or protected constructor.</typeparam>
        /// <param name="recordset"></param>
        /// <returns></returns>
        public static TInterface[] ToArray<TInterface, TImplentation>(this Recordset recordset) where TImplentation : class, TInterface
        {
            var interfaceType = typeof(TInterface);
            if (!interfaceType.IsInterface) throw new OrmException($"Generic argument TInterface resolves to {interfaceType.FullName} which is not an interface.");

            var instanceType = typeof(TImplentation);
            if (instanceType.GetInterface(interfaceType.Name) == null) throw new OrmException($"Genertic argument TImplentation{instanceType.FullName} must implement interface {interfaceType.FullName}");

            var result = new TInterface[recordset.Count];
            for (var i = 0; i < recordset.Count; ++i)
            {
                result[i] = recordset.ToObject<TImplentation>(i);
            }
            return result;
        }

        public static T[] ToArray<T>(this Recordset recordset) where T : class
        {
            var result = new T[recordset.Count];
            for (var i = 0; i < recordset.Count; ++i)
            {
                result[i] = recordset.ToObject<T>(i);
            }
            return result;
        }

        public static Task<T[]> ToArrayAsync<T>(this Recordset recordset) where T : class
        {
            return Task.Run(() =>
            {
                var result = new T[recordset.Count];
                for (var i = 0; i < recordset.Count; ++i)
                {
                    result[i] = recordset.ToObject<T>(i);
                }
                return result;
            });
        }

        public static T[] ToArrayParallel<T>(this Recordset recordset) where T : class
        {
            var result = new T[recordset.Count];
            Parallel.For(0, recordset.Count, i =>
            {
                result[i] = recordset.ToObject<T>(i);
            });
            return result;
        }
    }
}

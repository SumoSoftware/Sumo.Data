using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace Sumo.Data.Orm
{
    public static class RecordsetExtensions
    {

        public static T ToObject<T>(Recordset recordset, Field[] fields, long recordIndex) where T : class
        {
            // allows activator to use non-public constructors
            var result = (T)Activator.CreateInstance(
                typeof(T),
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                null, null, null);
            //var result = Activator.CreateInstance<T>();
            for (var i = 0; i < TypeInfoCache<T>.Properties.Length; ++i)
            {
                var property = TypeInfoCache<T>.Properties[i];
                var value =  
                if (!field.IsNull(property.Name))
                {
                    //todo: this can be optimized by passing in the table definition or a cache of row types from the ToArray methods
                    //todo: this can be optimized by getting the underlying nullable types in TypeInfoCache 
                    var columnValue = field[property.Name];
                    var isNullable = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                    var propertyType = isNullable ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;
                    var value = propertyType == columnValue.GetType() ?
                        columnValue :
                        Convert.ChangeType(columnValue, property.PropertyType);
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
                result[i] = recordset[i][0];
            }
            return result;
        }

        /// <summary>
        /// for value types like string, datetime, and numeric types 
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
                var isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
                type = isNullable ? Nullable.GetUnderlyingType(type) : type;
                var doConvert = type != recordset[0][0].GetType();
                for (var i = 0; i < recordset.Count; ++i)
                {
                    result[i] = (T)(doConvert ? recordset[i][0] : Convert.ChangeType(recordset[i][0], type));
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
            var result = new TInterface[recordset.Count];
            for (var i = 0; i < recordset.Count; ++i)
            {
                result[i] = recordset[i].ToObject<TImplentation>();
            }
            return result;
        }

        public static T[] ToArray<T>(this Recordset recordset) where T : class
        {
            var result = new T[recordset.Count];
            for (var i = 0; i < recordset.Count; ++i)
            {
                result[i] = recordset[i].ToObject<T>();
            }
            return result;
        }

        public static async Task<T[]> ToArrayAsync<T>(this Recordset recordset) where T : class
        {
            return await Task.Run(() =>
            {
                var result = new T[recordset.Count];
                for (var i = 0; i < recordset.Count; ++i)
                {
                    result[i] = recordset[i].ToObject<T>();
                }
                return result;
            });
        }

        public static T[] ToArrayParallel<T>(this Recordset recordset) where T : class
        {
            var result = new T[recordset.Count];
            Parallel.For(0, recordset.Count, i =>
            {
                result[i] = recordset[i].ToObject<T>();
            });
            return result;
        }
    }
}

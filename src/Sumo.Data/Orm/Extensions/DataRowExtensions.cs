using Sumo.Data.Orm;
using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

// this namespace is correct. don't add .Orm to it.
namespace Sumo.Data
{
    public static class DataRowExtensions
    {
        public static T ToObject<T>(this DataRow row) where T : class
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
                if (!row.IsNull(propertyName))
                {
                    var value = row[propertyName];
                    if(value != null)
                    {
                        //todo: this can be optimized by passing in the table definition or a cache of row types from the ToArray methods
                        //todo: this can be optimized by getting the underlying nullable types in TypeInfoCache 
                        var propertyType = property.PropertyType.IsNullable() ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;
                        value = propertyType == value.GetType() ? value : Convert.ChangeType(value, propertyType);
                        property.SetValue(result, value);
                    }
                }
            }
            return result;
        }

        //public static T ToObject<T>(this DataRow row, types) where T : class
        //{
        //    // allows activator to use non-public constructors
        //    var result = (T)Activator.CreateInstance(
        //        typeof(T),
        //        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
        //        null, null, null);
        //    //var result = Activator.CreateInstance<T>();
        //    for (var i = 0; i < TypeInfoCache<T>.Properties.Length; ++i)
        //    {
        //        var property = TypeInfoCache<T>.Properties[i];
        //        if (!row.IsNull(property.Name))
        //        {
        //            var columnValue = row[property.Name];
        //            columnValue.
        //            var value = Convert.ChangeType(row[property.Name], property.PropertyType);
        //            property.SetValue(result, value);
        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// for binary types
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static object[] ToObjectArray(this DataRowCollection rows)
        {
            var result = new object[rows.Count];
            for (var i = 0; i < rows.Count; ++i)
            {
                result[i] = rows[i][0];
            }
            return result;
        }

        /// <summary>
        /// for value/scalor types like string, datetime, and numeric types 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static T[] ToValueArray<T>(this DataRowCollection rows) where T : struct
        {
            var result = new T[rows.Count];
            if (rows.Count > 0)
            {
                var type = typeof(T);
                type = type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
                var doConvert = type != rows[0][0].GetType();
                for (var i = 0; i < rows.Count; ++i)
                {
                    result[i] = (T)(doConvert ? Convert.ChangeType(rows[i][0], type) : rows[i][0]);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplentation">The type to use to for the interface implementation. Must have public or protected constructor.</typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static TInterface[] ToArray<TInterface, TImplentation>(this DataRowCollection rows) where TImplentation : class, TInterface
        {
            var interfaceType = typeof(TInterface);
            if (!interfaceType.IsInterface) throw new OrmException($"Generic argument TInterface resolves to {interfaceType.FullName} which is not an interface.");

            var instanceType = typeof(TImplentation);
            if (instanceType.GetInterface(interfaceType.Name) == null) throw new OrmException($"Genertic argument TImplentation{instanceType.FullName} must implement interface {interfaceType.FullName}");

            var result = new TInterface[rows.Count];
            for (var i = 0; i < rows.Count; ++i)
            {
                result[i] = rows[i].ToObject<TImplentation>();
            }
            return result;
        }

        public static T[] ToArray<T>(this DataRowCollection rows) where T : class
        {
            var result = new T[rows.Count];
            for (var i = 0; i < rows.Count; ++i)
            {
                result[i] = rows[i].ToObject<T>();
            }
            return result;
        }

        public static async Task<T[]> ToArrayAsync<T>(this DataRowCollection rows) where T : class
        {
            return await Task.Run(() =>
            {
                var result = new T[rows.Count];
                for (var i = 0; i < rows.Count; ++i)
                {
                    result[i] = rows[i].ToObject<T>();
                }
                return result;
            });
        }

        public static T[] ToArrayParallel<T>(this DataRowCollection rows) where T : class
        {
            var result = new T[rows.Count];
            Parallel.For(0, rows.Count, i =>
            {
                result[i] = rows[i].ToObject<T>();
            });
            return result;
        }
    }
}

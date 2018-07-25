using Sumo.Data.Orm.Attributes;
using Sumo.Data.Types;
using System.Linq;
using System.Reflection;

namespace Sumo.Data.Orm.Types
{
    public static class EntityInfoCache<T> where T : class
    {
        static EntityInfoCache()
        {
            var type = typeof(T);

            PrimaryKeyProperties = TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                .ToArray();

            NonAutoIncrementProperties = TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() == null || p.GetCustomAttribute<PrimaryKeyAttribute>().AutoIncrement == false)
                .ToArray();

            UniqueProperties = TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<UniqueAttribute>() != null)
                .ToArray();

            RequiredProperties = TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<RequiredAttribute>() != null)
                .ToArray();

            //EntityWriteParameterNames = NonAutoIncrementProperties.Select((p) => new SqliteParameterName(p.Name).ToString()).ToArray();
        }

        public readonly static PropertyInfo[] PrimaryKeyProperties;
        public readonly static PropertyInfo[] NonAutoIncrementProperties;
        //public readonly static string[] EntityWriteParameterNames;
        public readonly static PropertyInfo[] UniqueProperties;
        public readonly static PropertyInfo[] RequiredProperties;
    }
}

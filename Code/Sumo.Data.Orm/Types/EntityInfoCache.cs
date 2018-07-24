using Sumo.Data.Orm.Attributes;
using System.Linq;
using System.Reflection;

namespace Sumo.Data.Orm.Types
{
    public static class EntityInfoCache<T> where T : class
    {
        static EntityInfoCache()
        {
            var type = typeof(T);

            PrimaryKeyProperties = Data.Types.TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                .ToArray();

            NonAutoIncrementProperties = Data.Types.TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() == null || p.GetCustomAttribute<PrimaryKeyAttribute>().AutoIncrement == false)
                .ToArray();

            UniqueProperties = Data.Types.TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<UniqueAttribute>() != null)
                .ToArray();

            RequiredProperties = Data.Types.TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<RequiredAttribute>() != null)
                .ToArray();
        }

        public readonly static PropertyInfo[] PrimaryKeyProperties;
        public readonly static PropertyInfo[] NonAutoIncrementProperties;
        public readonly static PropertyInfo[] UniqueProperties;
        public readonly static PropertyInfo[] RequiredProperties;
    }
}

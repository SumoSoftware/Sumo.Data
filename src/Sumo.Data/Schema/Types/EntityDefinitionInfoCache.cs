using System.Linq;
using System.Reflection;

namespace Sumo.Data.Schema
{
    public static class EntityDefinitionInfoCache<T> where T : class
    {
        static EntityDefinitionInfoCache()
        {
            PrimaryKeyProperties = TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                .OrderBy(p => p.Name)
                .ToArray();

            NonAutoIncrementProperties = TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() == null || p.GetCustomAttribute<PrimaryKeyAttribute>().AutoIncrement == false)
                .OrderBy(p => p.Name)
                .ToArray();

            UniqueProperties = TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<UniqueAttribute>() != null)
                .OrderBy(p => p.Name)
                .ToArray();

            RequiredProperties = TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<RequiredAttribute>() != null)
                .OrderBy(p => p.Name)
                .ToArray();
        }

        public readonly static PropertyInfo[] PrimaryKeyProperties;
        public readonly static PropertyInfo[] NonAutoIncrementProperties;
        public readonly static PropertyInfo[] UniqueProperties;
        public readonly static PropertyInfo[] RequiredProperties;
    }
}

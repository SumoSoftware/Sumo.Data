using Sumo.Data.Attributes;
using Sumo.Data.Names;
using System.Linq;
using System.Reflection;

namespace Sumo.Data.Types.SqlServer
{
    internal static class SqlServerEntityInfoCache<T> where T : class
    {
        static SqlServerEntityInfoCache()
        {
            var nonAutoIncrementProperties = TypeInfoCache<T>
                .Properties
                .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() == null || p.GetCustomAttribute<PrimaryKeyAttribute>().AutoIncrement == false)
                .OrderBy(p => p.Name)
                .ToArray();

            EntityWriteParameterNames = nonAutoIncrementProperties
                .OrderBy(p => p.Name)
                .Select((p) => p.Name)
                .ToArray();
        }

        public readonly static string[] EntityWriteParameterNames;
    }
}

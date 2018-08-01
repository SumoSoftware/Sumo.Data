using Sumo.Data.Schema.Attributes;
using Sumo.Data.Types;
using System.Linq;
using System.Reflection;

namespace Sumo.Data.Schema.Sqlite.Types
{
    internal static class SqliteEntityInfoCache<T> where T : class
    {
        static SqliteEntityInfoCache()
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

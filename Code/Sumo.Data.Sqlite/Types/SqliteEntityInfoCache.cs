using Sumo.Data.Attributes;
using Sumo.Data.Names.Sqlite;
using System.Linq;
using System.Reflection;

namespace Sumo.Data.Types.Sqlite
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
                .Select((p) => new SqliteParameterName(p.Name).ToString())
                .ToArray();
        }

        public readonly static string[] EntityWriteParameterNames;
    }
}

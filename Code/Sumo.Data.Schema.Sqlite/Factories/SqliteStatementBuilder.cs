using Sumo.Data.Factories;
using Sumo.Data.Names.Sqlite;
using Sumo.Data.Types;
using System.Collections.Generic;
using System.Text;

namespace Sumo.Data.Schema.Factories.Sqlite
{
    // if we have problems in android this might be why - can't pass non-string values as parameters?
    //https://stackoverflow.com/questions/18746149/android-sqlite-selection-args-with-int-values

    //todo: add support for an expression based select statement to ISqlStatementBuilder
    //https://www.sqlite.org/fileformat2.html#storage_of_the_sql_database_schema
    public class SqliteStatementBuilder : ISqlStatementBuilder
    {
        public string GetExistsStatement<T>() where T : class
        {
            return $"select case when exists (select * from [sqlite_master] where [tbl_name]='{TypeInfoCache<T>.Name}' and [type]='table') then 1 else 0 end as [exists]";
        }

        public string GetInsertStatement<T>() where T : class
        {
            var builder = new StringBuilder();

            builder.Append($"insert into [{TypeInfoCache<T>.Name}] (");
            for (var i = 0; i < EntityInfoCache<T>.NonAutoIncrementProperties.Length; ++i)
            {
                if (i > 0) builder.Append(", ");
                builder.Append(EntityInfoCache<T>.NonAutoIncrementProperties[i].Name);
            }
            builder.Append(") values (");
            for (var i = 0; i < EntityInfoCache<T>.NonAutoIncrementProperties.Length; ++i)
            {
                if (i > 0) builder.Append(", ");
                builder.Append(new SqliteParameterName(EntityInfoCache<T>.NonAutoIncrementProperties[i].Name));
            }
            builder.Append(");");

            return builder.ToString();
        }
        
        public string GetSelectStatement<T>(Dictionary<string, object> parameters) where T : class
        {
            var builder = new StringBuilder();

            builder.Append($"select ");
            for (var i = 0; i < TypeInfoCache<T>.ReadWriteProperties.Length; ++i)
            {
                if (i > 0) builder.Append(", ");
                builder.Append($"[{TypeInfoCache<T>.ReadWriteProperties[i].Name}]");
            }
            builder.Append($" from [{TypeInfoCache<T>.Name}] where ");
            for (var i = 0; i < TypeInfoCache<T>.ReadWriteProperties.Length; ++i)
            {
                if (i > 0) builder.Append(" and ");
                builder.Append($"[{TypeInfoCache<T>.ReadWriteProperties[i].Name}]={new SqliteParameterName(EntityInfoCache<T>.NonAutoIncrementProperties[i].Name)}");
            }

            return builder.ToString();
        }
    }
}

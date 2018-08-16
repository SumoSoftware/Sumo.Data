using System;
using System.Collections.Generic;
using System.Text;

namespace Sumo.Data.Schema.Sqlite
{
    // if we have problems in android this might be why - can't pass non-string values as parameters?
    //https://stackoverflow.com/questions/18746149/android-sqlite-selection-args-with-int-values

    //todo: add support for an expression based select statement to ISqlStatementBuilder
    //https://www.sqlite.org/fileformat2.html#storage_of_the_sql_database_schema
    public class SqliteStatementBuilder : ISqlStatementBuilder
    {
        public SqliteStatementBuilder(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
        }

        private readonly IParameterFactory _parameterFactory;


        public string GetExistsStatement<T>() where T : class
        {
            return $"select case when exists (select * from [sqlite_master] where [tbl_name]='{TypeInfoCache<T>.Name}' and [type]='table') then 1 else 0 end as [exists]";
        }

        public string GetInsertStatement<T>() where T : class
        {
            var builder = new StringBuilder();

            builder.Append($"insert into [{TypeInfoCache<T>.Name}] (");
            for (var i = 0; i < EntityDefinitionInfoCache<T>.NonAutoIncrementProperties.Length; ++i)
            {
                if (i > 0) builder.Append(", ");
                builder.Append(EntityDefinitionInfoCache<T>.NonAutoIncrementProperties[i].Name);
            }
            builder.Append(") values (");
            for (var i = 0; i < EntityDefinitionInfoCache<T>.NonAutoIncrementProperties.Length; ++i)
            {
                if (i > 0) builder.Append(", ");
                var name = _parameterFactory.GetParameterName(EntityDefinitionInfoCache<T>.NonAutoIncrementProperties[i].Name, i - 1);
                builder.Append(name);
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

            if (parameters.Count > 0)
            {
                builder.Append($" from [{TypeInfoCache<T>.Name}] where ");
                var index = -1;
                foreach (var kvp in parameters)
                {
                    if (index > 0) builder.Append(" and ");
                    var parameterName = _parameterFactory.GetParameterName(kvp.Key, index);
                    builder.Append($"[{kvp.Key}]={parameterName}");
                }
            }
            else
            {
                builder.Append($" from  [{TypeInfoCache<T>.Name}]");
            }

            return builder.ToString();
        }
    }
}

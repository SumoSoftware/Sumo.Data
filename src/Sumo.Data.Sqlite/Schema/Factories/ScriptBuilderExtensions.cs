using System;
using System.Data;
using System.Linq;
using System.Text;

namespace Sumo.Data.Schema.Sqlite
{
    // sqlite keywords reference: https://www.sqlite.org/lang_keywords.html

    // currently only supporting 'MAIN' schema
    // read schema-name stufff here: https://www.sqlite.org/lang_createtable.html
    // schema name that isn't 'MAIN' must specify an attached database
    // and read attached database stuff here: https://www.sqlite.org/lang_attach.html
    
    // todo: add attached database support
    internal static class ScriptBuilderExtensions
    {
        internal static string ToCreateScript(this Catalog catalog, bool checkExists = false)
        {
            var builder = new StringBuilder();

            if (catalog.Comments != null)
            {
                builder.Append("-- CATALOG");
                if (!string.IsNullOrEmpty(catalog.Name)) builder.Append($" [{catalog.Name.ToUpper()}]");
                builder.AppendLine(" COMMENTS");
                foreach (var comment in catalog.Comments)
                {
                    builder.AppendLine($"-- {comment}");
                }
                builder.AppendLine();
            }

            if (catalog.Schemas != null)
            {
                builder.Append("-- CATALOG");
                if (!string.IsNullOrEmpty(catalog.Name)) builder.Append($" [{catalog.Name.ToUpper()}]");
                builder.AppendLine(" SCHEMAS");
                builder.AppendLine();

                foreach (var schema in catalog.Schemas)
                {
                    builder.AppendLine(schema.ToCreateScript(checkExists));
                    builder.AppendLine();
                }
            }
            return builder.ToString();
        }

        internal static string ToCreateScript(this Schema schema, bool checkExists = false)
        {
            if (!string.IsNullOrEmpty(schema.Name) && (schema.Name.ToLower().CompareTo("main") != 0 && schema.Name.ToLower().CompareTo("temp") != 0))
                throw new ArgumentException($"{nameof(schema)}.{nameof(schema.Name)} must be empty, 'main', or 'temp'.");

            if (string.IsNullOrEmpty(schema.Name)) schema.Name = "main";

            var builder = new StringBuilder();

            if (schema.Comments != null)
            {
                builder.AppendLine($"-- SCHEMA [{schema.Name.ToUpper()}] COMMENTS");
                foreach (var comment in schema.Comments)
                {
                    builder.AppendLine($"-- {comment}");
                }
                builder.AppendLine();
            }

            if (schema.Tables != null)
            {
                builder.AppendLine($"-- SCHEMA [{schema.Name.ToUpper()}] TABLES");
                foreach (var table in schema.Tables)
                {
                    builder.AppendLine(table.ToCreateScript(schema, checkExists));
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }


        internal static string ToCreateScript(this Table table, bool checkExists = false)
        {
            return table.ToCreateScript(null, checkExists);
        }

        //https://www.sqlite.org/lang_createtable.html
        internal static string ToCreateScript(this Table table, Schema schema, bool checkExists = false)
        {
            if (string.IsNullOrEmpty(table.Name)) throw new ArgumentException($"{table} {nameof(Table)}.{nameof(Table.Name)} cannot be null or empty.");

            var builder = new StringBuilder();

            builder.AppendLine($"-- CREATE TABLE [{table.Name.ToUpper()}]");
            if (table.Comments != null)
                foreach (var comment in table.Comments)
                    builder.AppendLine($"-- {comment}");

            builder.AppendLine();
            builder.Append("create");
            if (schema != null && schema.Name.ToLower().CompareTo("temp") == 0) builder.Append(" temp");
            builder.Append(" table");
            if (checkExists) builder.Append(" if not exists");
            if (schema != null)
                builder.AppendLine($" [{schema.Name}].[{table.Name}] (");
            else
                builder.AppendLine($" [{table.Name}] (");

            if (table.Columns != null)
            {
                var columns = table.Columns.OrderBy((c) => c.OrdinalPosition);
                foreach (var column in columns)
                {
                    if (column.OrdinalPosition > 1) builder.AppendLine(",");
                    builder.Append("  ");
                    builder.Append(column.ToCreateScript());
                }
            }
            if (table.HasCheckConstraint)
                builder.Append($" check ({table.CheckConstraint.Expression})");
            builder.AppendLine(")");

            if (table.Indexes != null)
            {
                builder.AppendLine();
                builder.AppendLine($"-- [{table.Name.ToUpper()}] INDEXES");
                builder.AppendLine();
                foreach (var index in table.Indexes)
                    builder.AppendLine(index.ToCreateScript(table, schema, checkExists));
            }

            return builder.ToString();
        }

        //todo: add comment support for columns
        //https://www.sqlite.org/datatype3.html
        //https://www.sqlite.org/syntax/column-constraint.html
        internal static string ToCreateScript(this Column column)
        {
            if (string.IsNullOrEmpty(column.Name)) throw new ArgumentException($"{column} {nameof(Column)}.{nameof(Column.Name)} cannot be null or empty.");

            var builder = new StringBuilder();

            // name and type
            builder.Append($"[{column.Name}]");
            var typeName = column.DataType.ToString();
            if (column.IsPrimaryKey)
            {
                // sqlite uses special case for PK and INTEGER - read up on rowid
                switch (column.DataType)
                {
                    case DbType.Int32:
                    case DbType.Int64:
                    case DbType.UInt32:
                    case DbType.UInt64:
                        typeName = "INTEGER";
                        break;
                }
            }
            builder.Append($" {typeName}");
            switch (column.DataType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.Binary:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                    if (column.MaxLength.HasValue) builder.Append($" ({column.MaxLength.Value.ToString()})");
                    break;
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Single:
                    if (column.Precision.HasValue) builder.Append($" ({column.Precision.Value})");
                    break;
            }

            // constraints
            if (column.IsPrimaryKey)
            {
                builder.Append($" primary key {column.PrimaryKey.Direction.ToSqlString().ToLower()}");
                if (column.PrimaryKey.ConflictClause.HasValue)
                {
                    builder.Append($" on conflict {column.PrimaryKey.ConflictClause.Value.ToString().ToLower()}");
                }
                if (column.PrimaryKey.IsAutoIncrement && false)
                {
                    builder.Append(" autoincrement");
                }
            }

            if (!column.IsNullable)
            {
                builder.Append(" not null");
                if (column.NotNullConstraint.ConflictClause.HasValue)
                {
                    builder.Append($" on conflict {column.NotNullConstraint.ConflictClause.Value.ToString().ToLower()}");
                }
            }

            if (column.IsUnique)
            {
                builder.Append(" unique");
                if (column.UniqueConstraint.ConflictClause.HasValue)
                {
                    builder.Append($" on conflict {column.UniqueConstraint.ConflictClause.Value.ToString().ToLower()}");
                }
            }

            if (column.HasCheckConstraint)
            {
                builder.Append($" check ({column.CheckConstraint.Expression})");
            }

            if (!string.IsNullOrEmpty(column.Default))
            {
                builder.Append($" default ({column.Default})");
            }

            if (!string.IsNullOrEmpty(column.CollationName))
            {
                builder.Append($" collate ({column.CollationName})");
            }

            if (column.HasForeignKey)
            {
                //https://www.sqlite.org/syntax/foreign-key-clause.html
                //todo: add cascade support
                builder.Append($" references [{column.ForeignKey.Table}] ([{column.ForeignKey.Column}])");
            }

            return builder.ToString();
        }

        internal static string ToCreateScript(this Index index, Table table, Schema schema, bool checkExists = false)
        {
            if (string.IsNullOrEmpty(index.Name)) throw new ArgumentException($"{index} {nameof(Index)}.{nameof(Index.Name)} cannot be null or empty.");

            var builder = new StringBuilder();

            builder.Append("create");
            if (index.IsUnique) builder.Append(" unique");
            builder.Append(" index");
            if (checkExists) builder.Append(" if not exists");
            if (schema != null)
                builder.Append($" [{schema.Name}].[{index.Name}] on [{table.Name}] (");
            else
                builder.Append($" [{index.Name}] on [{table.Name}] (");

            if (index.IndexedColumns != null)
            {
                var columns = index.IndexedColumns.OrderBy((c) => c.OrdinalPosition);
                foreach (var column in columns)
                {
                    builder.Append(column.OrdinalPosition > 1 ? ", " : string.Empty);
                    builder.Append(column.ToCreateScript(table));
                }
            }
            builder.Append(")");
            if (!string.IsNullOrEmpty(index.FilterExpression)) builder.Append($" where {index.FilterExpression}");

            return builder.ToString();
        }

        internal static string ToCreateScript(this IndexedColumn column, Table table)
        {
            if (string.IsNullOrEmpty(column.Name)) throw new ArgumentException($"{column} {nameof(IndexedColumn)}.{nameof(IndexedColumn.Name)} cannot be null or empty.");

            var builder = new StringBuilder();

            builder.Append($"[{column.Name}]");
            if (!string.IsNullOrEmpty(column.CollationName)) builder.Append($"collate {column.CollationName}");
            builder.Append($" {column.Direction.ToSqlString()}");

            return builder.ToString();
        }
    }
}

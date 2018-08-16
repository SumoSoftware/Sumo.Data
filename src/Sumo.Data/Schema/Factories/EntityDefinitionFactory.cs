using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sumo.Data.Schema
{
    public class EntityDefinitionFactory
    {
        public CatalogDefinition ToCatalog(DataSet dataset)
        {
            var schema = ToSchema(dataset.Tables);
            var catalog = new CatalogDefinition();
            catalog.AddSchema(schema);
            return catalog;
        }

        public SchemaDefinition ToSchema(DataTableCollection tables, string owner = "dbo")
        {
            var schema = new SchemaDefinition();
            for (var idx = 0; idx < tables.Count; ++idx)
            {
                schema.AddTable(ToTable(tables[idx]));
            }
            return schema;
        }

        public SchemaDefinition ToSchema(IEnumerable<DataTable> tables, string owner = "dbo")
        {
            var schema = new SchemaDefinition();
            foreach (var table in tables)
            {
                schema.AddTable(ToTable(table));
            }
            return schema;
        }

        public TableDefinition ToTable(DataTable table, string tableName = "", string owner = "dbo")
        {
            var outputTable = new TableDefinition
            {
                Name = string.IsNullOrEmpty(tableName) ? table.TableName : tableName
            };

            foreach (DataColumn col in table.Columns)
            {
                outputTable.AddColumn(ToColumn(col));
            }

            var keys = table.PrimaryKey;
            foreach (var key in keys)
            {
                var keyColumn = outputTable.Columns.Where(col => col.Name == key.ColumnName).FirstOrDefault();
                if (keyColumn != null)
                {
                    keyColumn.PrimaryKey = new PrimaryKeyDefinition()
                    {
                        IsAutoIncrement = key.AutoIncrement,
                        Seed = key.AutoIncrementSeed,
                        Step = key.AutoIncrementStep
                    };
                }
            }

            return outputTable;
        }

        public ColumnDefinition ToColumn(DataColumn column)
        {
            var col = new ColumnDefinition
            {
                Name = column.ColumnName,
                IsNullable = column.AllowDBNull,

                Default = column.DefaultValue?.ToString(),
                OrdinalPosition = column.Ordinal,
                DataType = column.DataType.ToDbType()
            };

            return col;
        }
    }
}

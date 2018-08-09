using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sumo.Data.Schema
{
    public class EntityFactory
    {
        public Catalog ToCatalog(DataSet dataset)
        {
            var schema = ToSchema(dataset.Tables);
            var catalog = new Catalog();
            catalog.AddSchema(schema);
            return catalog;
        }

        public Schema ToSchema(DataTableCollection tables, string owner = "dbo")
        {
            var schema = new Schema();
            for (var idx = 0; idx < tables.Count; ++idx)
            {
                schema.AddTable(ToTable(tables[idx]));
            }
            return schema;
        }

        public Schema ToSchema(IEnumerable<DataTable> tables, string owner = "dbo")
        {
            var schema = new Schema();
            foreach (var table in tables)
            {
                schema.AddTable(ToTable(table));
            }
            return schema;
        }

        public Table ToTable(DataTable table, string tableName = "", string owner = "dbo")
        {
            var outputTable = new Table
            {
                Name = String.IsNullOrEmpty(tableName) ? table.TableName : tableName
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
                    keyColumn.PrimaryKey = new PrimaryKey()
                    {
                        IsAutoIncrement = key.AutoIncrement,
                        Seed = key.AutoIncrementSeed,
                        Step = key.AutoIncrementStep
                    };
                }
            }

            return outputTable;
        }

        public Column ToColumn(DataColumn column)
        {
            var col = new Column
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

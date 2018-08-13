using System;
using System.Data;

namespace Sumo.Data.Core.Datasets
{
    public class Dataset
    {
        public Dataset() : base() { }

        public Dataset(string name) : base()
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public Dataset(string name, Column[] columns) : this(name)
        {
            Columns = columns;
        }

        public Dataset(string name, Column[] columns, Row[] rows) : this(name, columns)
        {
            Rows = rows;
        }

        public Dataset(DataTable table) : this(table.TableName)
        {
            Columns = new Column[table.Columns.Count];
            for(var i = 0; i < table.Columns.Count; ++i)
            {
                Columns[i] = new Column(table.Columns[i]);
            }

            Rows = new Row[table.Rows.Count];
            for (var i = 0; i < table.Columns.Count; ++i)
            {
                Rows[i] = new Row(table.Rows[i]);
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Column[] Columns { get; set; }
        public Row[] Rows { get; set; }
    }
}

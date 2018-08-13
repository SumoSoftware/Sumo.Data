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

        public Dataset(string name, string[] columns, object[][] rows) : this(name)
        {
            var typeCodes = new TypeCode[columns.Length];
            for(var i=0; i<columns.Length; ++i)
            {
                for(var j = 0; j < rows.Length; ++j)
                {
                    var item= rows[j][i];
                    var typeCode = item == null ? TypeCode.Empty : Type.GetTypeCode(item.GetType());
                    if(typeCode != TypeCode.Empty)
                    {
                        typeCodes[i] = typeCode;
                        break;
                    }
                }
            }

            Columns = new Column[columns.Length];
            for (var i = 0; i < columns.Length; ++i)
            {
                Columns[i] = new Column(columns[i], typeCodes[i], i);
            }

            Rows = new Row[rows.Length];
            for (var i = 0; i < rows.Length; ++i)
            {
                Rows[i] = new Row(rows[i]);
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Column[] Columns { get; set; }
        public Row[] Rows { get; set; }
    }
}

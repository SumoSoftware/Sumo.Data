using System;
using System.Data;

namespace Sumo.Data.Datasets
{
    public class Recordset
    {
        public Recordset() : base() { }

        public Recordset(string name) : base()
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public Recordset(string name, Field[] columns) : this(name)
        {
            Columns = columns;
        }

        public Recordset(string name, Field[] columns, Record[] rows) : this(name, columns)
        {
            Rows = rows;
        }

        public Recordset(DataTable table) : this(table.TableName)
        {
            Columns = new Field[table.Columns.Count];
            for(var i = 0; i < table.Columns.Count; ++i)
            {
                Columns[i] = new Field(table.Columns[i]);
            }

            Rows = new Record[table.Rows.Count];
            for (var i = 0; i < table.Columns.Count; ++i)
            {
                Rows[i] = new Record(table.Rows[i]);
            }
        }

        public Recordset(string name, string[] columns, object[][] rows) : this(name)
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

            Columns = new Field[columns.Length];
            for (var i = 0; i < columns.Length; ++i)
            {
                Columns[i] = new Field(columns[i], typeCodes[i], i);
            }

            Rows = new Record[rows.Length];
            for (var i = 0; i < rows.Length; ++i)
            {
                Rows[i] = new Record(rows[i]);
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Field[] Columns { get; set; }
        public Record[] Rows { get; set; }
    }
}

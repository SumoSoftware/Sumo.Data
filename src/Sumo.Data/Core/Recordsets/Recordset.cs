using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sumo.Data
{
    public class Recordset
    {
        public Recordset() : base() { }

        public Recordset(string name) : base()
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public Recordset(string name, Field[] fields) : this(name)
        {
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public Recordset(string name, Field[] fields, object[][] records) : this(name, fields)
        {
            Records = records ?? throw new ArgumentNullException(nameof(records));
        }

        public Recordset(DataTable table) : this(table?.TableName)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            Fields = new Field[table.Columns.Count];
            for (var i = 0; i < table.Columns.Count; ++i)
            {
                Fields[i] = new Field(table.Columns[i]);
            }

            Records = new object[table.Rows.Count][];
            for (var i = 0; i < table.Rows.Count; ++i)
            {
                Records[i] = table.Rows[i].ItemArray;
            }
        }

        public Recordset(string name, string[] fields, List<List<object>> records) : this(name)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (records == null) throw new ArgumentNullException(nameof(records));

            Records = new object[records.Count][];
            for (var i = 0; i < records.Count; ++i)
            {
                Records[i] = records[i].ToArray();
            }

            var typeCodes = GetTypeCodes(fields.Length);

            Fields = new Field[fields.Length];
            for (var i = 0; i < fields.Length; ++i)
            {
                Fields[i] = new Field(fields[i], typeCodes[i], i);
            }
        }

        public Recordset(string name, string[] fields, object[][] records) : this(name)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            Records = records ?? throw new ArgumentNullException(nameof(records));

            var typeCodes = GetTypeCodes(fields.Length);
            Fields = new Field[fields.Length];
            for (var i = 0; i < fields.Length; ++i)
            {
                Fields[i] = new Field(fields[i], typeCodes[i], i);
            }
        }

        public Recordset(string name, object[][] records) : this(name)
        {
            Records = records ?? throw new ArgumentNullException(nameof(records));

            if (records.Length > 0)
            {
                var fieldCount = records[0].Length;
                var typeCodes = GetTypeCodes(fieldCount);
                Fields = new Field[fieldCount];
                for (var i = 0; i < fieldCount; ++i)
                {
                    Fields[i] = new Field($"field_{i}_{typeCodes[i]}", typeCodes[i], i);
                }
            }
        }

        private TypeCode[] GetTypeCodes(int fieldCount)
        {
            var result = new TypeCode[fieldCount];
            // iterate columns to file a list with types to be assigned to the fields int he next step
            for (var colIndex = 0; colIndex < fieldCount; ++colIndex)
            {
                // iterate the rows until the a type is found for field[i]
                for (var rowIndex = 0; rowIndex < Records.Length; ++rowIndex)
                {
                    var item = Records[rowIndex][colIndex]; // row j, col i
                    var typeCode = item == null ? TypeCode.Empty : Type.GetTypeCode(item.GetType());
                    if (typeCode != TypeCode.Empty)
                    {
                        result[colIndex] = typeCode;
                        break;
                    }
                }
            }
            return result;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Field[] Fields { get; set; }
        public object[][] Records { get; set; }

        [JsonIgnore]
        public object[] this[int index] => Records[index];
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Sumo.Data
{
    public class PortableRecordset
    {
        public PortableRecordset() : base() { }

        public PortableRecordset(string name) : base()
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public PortableRecordset(string name, Field[] fields) : this(name)
        {
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public PortableRecordset(string name, Field[] fields, object[][] records) : this(name, fields)
        {
            Records = records ?? throw new ArgumentNullException(nameof(records));
        }

        public PortableRecordset(DataTable table) : this(table?.TableName)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            _fields = new Field[table.Columns.Count];
            for (var i = 0; i < table.Columns.Count; ++i)
            {
                _fields[i] = new Field(table.Columns[i]);
            }
            Fields = _fields;

            Records = new object[table.Rows.Count][];
            for (var i = 0; i < table.Rows.Count; ++i)
            {
                Records[i] = new object[table.Columns.Count];
                for (var j = 0; j < table.Columns.Count; ++j)
                {
                    Records[i][j] = table.Rows[i].IsNull(j) ? null : table.Rows[i][j];
                }
            }
        }

        public PortableRecordset(string name, string[] fields, List<List<object>> records) : this(name)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (records == null) throw new ArgumentNullException(nameof(records));

            Records = new object[records.Count][];
            for (var i = 0; i < records.Count; ++i)
            {
                Records[i] = records[i].ToArray();
            }

            var typeCodes = GetTypeCodes(fields.Length);

            _fields = new Field[fields.Length];
            for (var i = 0; i < fields.Length; ++i)
            {
                _fields[i] = new Field(fields[i], typeCodes[i], i);
            }
            Fields = _fields;
        }

        public PortableRecordset(string name, string[] fields, object[][] records) : this(name)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            Records = records ?? throw new ArgumentNullException(nameof(records));

            var typeCodes = GetTypeCodes(fields.Length);

            _fields = new Field[fields.Length];
            for (var i = 0; i < fields.Length; ++i)
            {
                _fields[i] = new Field(fields[i], typeCodes[i], i);
            }
            Fields = _fields;
        }

        public PortableRecordset(string name, object[][] records) : this(name)
        {
            Records = records ?? throw new ArgumentNullException(nameof(records));

            if (records.Length > 0)
            {
                var fieldCount = records[0].Length;
                var typeCodes = GetTypeCodes(fieldCount);

                _fields = new Field[fieldCount];
                for (var i = 0; i < fieldCount; ++i)
                {
                    _fields[i] = new Field($"field_{i}", typeCodes[i], i);
                }
                Fields = _fields;
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

        public int Append(object[][] records)
        {
            if (records == null) throw new ArgumentNullException(nameof(records));

            var newrows = new object[Records.Length + records.Length][];

            Records.CopyTo(newrows, 0);
            records.CopyTo(newrows, Records.Length);
            Records = newrows;

            return records.Length;
        }

        public int Append(List<List<object>> records)
        {
            if (records == null) throw new ArgumentNullException(nameof(records));

            var newRecordCount = Records.Length + records.Count;
            var newrows = new object[newRecordCount][];

            Records.CopyTo(newrows, 0);

            for (var i = Records.Length; i < newRecordCount; ++i)
            {
                newrows[i] = records[i - Records.Length].ToArray();
            }

            Records = newrows;

            return records.Count;
        }

        private Dictionary<string, int> _fieldIndexes;

        public Guid Id { get; set; }

        public string Name { get; set; }

        private Field[] _fields;
        public Field[] Fields
        {
            get => _fields;
            set
            {
                _fields = value;
                _fieldIndexes = new Dictionary<string, int>(_fields.Length);
                for (var i = 0; i < _fields.Length; ++i)
                {
                    var field = _fields[i];
                    _fieldIndexes.Add(field.Name, field.OrdinalPosition);
                }
            }
        }
        public object[][] Records { get; set; }

        public long Count => Records.Length;

        public int FieldCount => Fields.Length;

        [JsonIgnore]
        public object[] this[long index] => Records[index];

        [JsonIgnore]
        public object this[long record, int field] => Records[record][field];

        [JsonIgnore]
        public object this[long index, string columnName] => Records[index][_fieldIndexes[columnName]];

        //todo: determine a philosophy on where stuff like ToBytes belongs - extensions, in class, or in partial class
        //todo: add binary support

        public byte[] ToBytes()
        {
            throw new NotImplementedException(nameof(ToBytes));
            //todo: use type code from fields to write rows to binary stream
        }

        public static PortableRecordset FromBytes(byte[] bytes)
        {
            throw new NotImplementedException(nameof(FromBytes));
            //todo: use type code from fields to read rows to binary stream
        }

        public static PortableRecordset FromStream(Stream bytes)
        {
            throw new NotImplementedException(nameof(FromBytes));
            //todo: use type code from fields to read rows to binary stream
        }
    }
}

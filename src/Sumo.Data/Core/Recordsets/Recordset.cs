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

        //public Recordset(string name, Field[] fields, Record[] records) : this(name, fields)
        //{
        //    Records = records;
        //}

        public Recordset(string name, Field[] fields, object[][] records) : this(name, fields)
        {
            Records = records ?? throw new ArgumentNullException(nameof(records));
        }

        //public Recordset(DataTable table) : this(table.TableName)
        //{
        //    Fields = new Field[table.Columns.Count];
        //    for(var i = 0; i < table.Columns.Count; ++i)
        //    {
        //        Fields[i] = new Field(table.Columns[i]);
        //    }

        //    Records = new Record[table.Rows.Count];
        //    for (var i = 0; i < table.Rows.Count; ++i)
        //    {
        //        Records[i] = new Record(table.Rows[i]);
        //    }
        //}

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

        //public Recordset(string name, string[] fields, List<List<object>> records) : this(name)
        //{
        //    var typeCodes = new TypeCode[fields.Length];
        //    for (var i = 0; i < fields.Length; ++i)
        //    {
        //        for (var j = 0; j < records.Count; ++j)
        //        {
        //            var item = records[j][i];
        //            var typeCode = item == null ? TypeCode.Empty : Type.GetTypeCode(item.GetType());
        //            if (typeCode != TypeCode.Empty)
        //            {
        //                typeCodes[i] = typeCode;
        //                break;
        //            }
        //        }
        //    }

        //    Fields = new Field[fields.Length];
        //    for (var i = 0; i < fields.Length; ++i)
        //    {
        //        Fields[i] = new Field(fields[i], typeCodes[i], i);
        //    }

        //    Records = new Record[records.Count];
        //    for (var i = 0; i < records.Count; ++i)
        //    {
        //        Records[i] = new Record(records[i]);
        //    }
        //}

        public Recordset(string name, string[] fields, List<List<object>> records) : this(name)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (records == null) throw new ArgumentNullException(nameof(records));

            var typeCodes = new TypeCode[fields.Length];
            for (var i = 0; i < fields.Length; ++i)
            {
                for (var j = 0; j < records.Count; ++j)
                {
                    var item = records[j][i];
                    var typeCode = item == null ? TypeCode.Empty : Type.GetTypeCode(item.GetType());
                    if (typeCode != TypeCode.Empty)
                    {
                        typeCodes[i] = typeCode;
                        break;
                    }
                }
            }

            Fields = new Field[fields.Length];
            for (var i = 0; i < fields.Length; ++i)
            {
                Fields[i] = new Field(fields[i], typeCodes[i], i);
            }

            Records = new object[records.Count][];
            for (var i = 0; i < records.Count; ++i)
            {
                Records[i] = records[i].ToArray();
            }
        }

        //public Recordset(string name, string[] fields, object[][] records) : this(name)
        //{
        //    var typeCodes = new TypeCode[fields.Length];
        //    for(var i=0; i<fields.Length; ++i)
        //    {
        //        for(var j = 0; j < records.Length; ++j)
        //        {
        //            var item= records[j][i];
        //            var typeCode = item == null ? TypeCode.Empty : Type.GetTypeCode(item.GetType());
        //            if(typeCode != TypeCode.Empty)
        //            {
        //                typeCodes[i] = typeCode;
        //                break;
        //            }
        //        }
        //    }

        //    Fields = new Field[fields.Length];
        //    for (var i = 0; i < fields.Length; ++i)
        //    {
        //        Fields[i] = new Field(fields[i], typeCodes[i], i);
        //    }

        //    Records = new Record[records.Length];
        //    for (var i = 0; i < records.Length; ++i)
        //    {
        //        Records[i] = new Record(records[i]);
        //    }
        //}

        public Recordset(string name, string[] fields, object[][] records) : this(name)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            Records = records ?? throw new ArgumentNullException(nameof(records));

            var typeCodes = new TypeCode[fields.Length];
            // iterate columns to file a list with types to be assigned to the fields int he next step
            for (var i = 0; i < fields.Length; ++i)
            {
                // iterate the rows until the a type is found for field[i]
                for (var j = 0; j < records.Length; ++j)
                {
                    var item = records[j][i]; // row j, col i
                    var typeCode = item == null ? TypeCode.Empty : Type.GetTypeCode(item.GetType());
                    if (typeCode != TypeCode.Empty)
                    {
                        typeCodes[i] = typeCode;
                        break;
                    }
                }
            }

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

                var typeCodes = new TypeCode[fieldCount];
                // iterate columns to file a list with types to be assigned to the fields int he next step
                for (var i = 0; i < fieldCount; ++i)
                {
                    // iterate the rows until the a type is found for field[i]
                    for (var j = 0; j < records.Length; ++j)
                    {
                        var item = records[j][i]; // row j, col i
                        var typeCode = item == null ? TypeCode.Empty : Type.GetTypeCode(item.GetType());
                        if (typeCode != TypeCode.Empty)
                        {
                            typeCodes[i] = typeCode;
                            break;
                        }
                    }
                }

                Fields = new Field[fieldCount];
                for (var i = 0; i < fieldCount; ++i)
                {
                    Fields[i] = new Field($"field_{i}_{typeCodes[i]}", typeCodes[i], i);
                }
            }
        }


        public Guid Id { get; set; }
        public string Name { get; set; }
        public Field[] Fields { get; set; }

        //public Record[] Records { get; set; }
        //todo: experiment with the idea of a two dimensional array of object for records
        public object[][] Records { get; set; }
    }
}

using Sumo.Data.Schema;
using System.Data;
using System.IO;
using System.Linq;

namespace Sumo.Data
{
    public static class BinaryExtensions
    {
        public static object[] ReadRowFromStream(this BinaryReader reader, TableDefinition table)
        {
            var row = new object[table.Columns.Count];
            foreach (var col in table.Columns.OrderBy(cols => cols.OrdinalPosition))
            {
                if (col.IsNullable)
                {
                    row[col.OrdinalPosition - 1] = (reader.ReadByte() == 1) ? reader.Read(col.DataType) : null;
                }
                else
                {
                    row[col.OrdinalPosition - 1] = reader.Read(col.DataType);
                }
            }

            return row;
        }

        public static void WriteToStream(this BinaryWriter writer, TableDefinition table, DataRow row)
        {
            //TODO: do an assertion on all ordinals not being zero
            foreach (var col in table.Columns.OrderBy(cols => cols.OrdinalPosition))
            {
                if (col.IsNullable)
                {
                    if (row.IsNull(col.Name))
                    {
                        writer.Write((byte)0);
                    }
                    else
                    {
                        writer.Write((byte)1);
                        var value = row[col.Name];
                        value.Write(writer, col.DataType);
                    }
                }
                else
                {
                    var value = row[col.Name];
                    value.Write(writer, col.DataType);
                }
            }
        }

        public static void WriteToStream(this BinaryWriter writer, TableDefinition table, DataRowCollection rows)
        {
            for (var idx = 0; idx < rows.Count; ++idx)
            {
                writer.WriteToStream(table, rows[idx]);
            }
        }
    }
}

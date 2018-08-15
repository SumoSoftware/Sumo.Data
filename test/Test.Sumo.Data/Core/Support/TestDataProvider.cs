using System;
using System.Data;

namespace Sumo.Data
{
    public static class TestDataProvider
    {
        public static DataRow GetRow()
        {
            DataRow row = null;
            using (var table = new DataTable())
            {
                for (var i = 0; i < TypeInfoCache<TestType>.Properties.Length; ++i)
                {
                    var property = TypeInfoCache<TestType>.Properties[i];
                    table.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                }
                row = table.NewRow();
                row["ReadWriteString"] = "row one";
                row["ReadWriteInteger"] = 1;
            }
            return row;
        }

        public static DataRowCollection GetRows()
        {
            DataRowCollection result = null;
            using (var table = new DataTable())
            {
                for (var i = 0; i < TypeInfoCache<TestType>.Properties.Length; ++i)
                {
                    var property = TypeInfoCache<TestType>.Properties[i];
                    table.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                }

                var row = table.NewRow();
                row["ReadWriteString"] = "row one";
                row["ReadWriteInteger"] = 1;
                table.Rows.Add(row);

                row = table.NewRow();
                row["ReadWriteString"] = "row two";
                row["ReadWriteInteger"] = 2;
                table.Rows.Add(row);

                result = table.Rows;
            }
            return result;
        }

        public static DataSet GetDataSet()
        {
            var table = new DataTable("TableName");
            for (var i = 0; i < TypeInfoCache<TestType>.Properties.Length; ++i)
            {
                var property = TypeInfoCache<TestType>.Properties[i];
                table.Columns.Add(new DataColumn(property.Name, property.PropertyType));
            }


            var row = table.NewRow();
            row["ReadPrivateWriteString"] = null;
            row["ReadString"] = null;
            row["ReadWriteString"] = "row one";
            row["ReadWriteInteger"] = 1;
            table.Rows.Add(row);

            row = table.NewRow();
            row["ReadPrivateWriteString"] = null;
            row["ReadString"] = null;
            row["ReadWriteString"] = "row two";
            row["ReadWriteInteger"] = 2;
            table.Rows.Add(row);

            var result = new DataSet("DataSetName");
            result.Tables.Add(table);
            return result;
        }

        public static DataSet GetDataSet<T>() where T : class
        {
            var table = new DataTable("TableName");
            for (var i = 0; i < TypeInfoCache<T>.Properties.Length; ++i)
            {
                var property = TypeInfoCache<T>.Properties[i];
                table.Columns.Add(new DataColumn(property.Name, property.PropertyType));
            }

            for (var j = 0; j < 2; ++j)
            {
                var row = table.NewRow();
                table.Rows.Add(row);
                for (var i = 0; i < TypeInfoCache<T>.Properties.Length; ++i)
                {
                    var property = TypeInfoCache<T>.Properties[i];
                    switch (TypeInfoCache<T>.TypeCodes[i])
                    {
                        case TypeCode.Boolean:
                            row[property.Name] = true;
                            break;
                        case TypeCode.SByte:
                        case TypeCode.Char:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Byte:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        case TypeCode.Decimal:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            row[property.Name] = i;
                            break;
                        case TypeCode.DateTime:
                            row[property.Name] = DateTime.Now;
                            break;
                        case TypeCode.String:
                            row[property.Name] = $"row {i}";
                            break;
                        case TypeCode.DBNull:
                        case TypeCode.Empty:
                        case TypeCode.Object:
                        default:
                            row[property.Name] = null;
                            break;
                    }
                }
            }

            var result = new DataSet("DataSetName");
            result.Tables.Add(table);
            return result;
        }
    }
}

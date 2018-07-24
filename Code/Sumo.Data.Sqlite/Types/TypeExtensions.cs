using Microsoft.Data.Sqlite;
using System;

namespace Sumo.Data.Types.Sqlite
{
    public static class TypeExtensions
    {
        public static SqliteType ToDbType(this string typeName)
        {
            if (!Enum.TryParse(typeName, true, out SqliteType sqliteType)) throw new ArgumentOutOfRangeException($"{nameof(typeName)}: '{typeName}'");
            return sqliteType;
        }

        public static SqliteType ToDbType(this Type type)
        {
            if (type == typeof(Guid))
                return SqliteType.Text;

            if (type.IsArray)
            {
                if (type == typeof(byte[]) || type == typeof(char[]))
                {
                    return SqliteType.Blob;
                }
                else
                {
                    return type.GetElementType().ToDbType();
                }
            }

            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Char:
                    return SqliteType.Integer;

                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                    return SqliteType.Real;

                case TypeCode.DateTime:
                case TypeCode.String:
                    return SqliteType.Text;

                case TypeCode.DBNull:
                case TypeCode.Empty:
                    throw new NotSupportedException($"{nameof(type)} can't be {typeCode}.");

                case TypeCode.Object:
                default:
                    return SqliteType.Blob;
            }
        }

        public static Type ToType(this SqliteType sqliteType)
        {
            switch (sqliteType)
            {
                case SqliteType.Integer:
                    return typeof(long);
                case SqliteType.Real:
                    return typeof(double);
                case SqliteType.Text:
                    return typeof(string);
                case SqliteType.Blob:
                    return typeof(byte[]);
                default:
                    return typeof(DBNull);
            }
        }

        public static SqliteAffinityTypes ToAffinityType(this SqliteType sqliteType)
        {
            return (SqliteAffinityTypes)sqliteType;
        }
    }
}

using Microsoft.Data.Sqlite;
using System;

namespace Sumo.Data.Sqlite
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
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Object:
                    if (type == typeof(Byte[]) || type == typeof(Object))
                    {
                        return SqliteType.Blob;
                    }
                    else if (type == typeof(Char[]) || type == typeof(Guid) || type == typeof(TimeSpan))
                    {
                        return SqliteType.Text;
                    }
                    throw new NotSupportedException(type.FullName);
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Char: return SqliteType.Integer;
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double: return SqliteType.Real;
                case TypeCode.DateTime:
                case TypeCode.String: return SqliteType.Text;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    throw new NotSupportedException($"{type.FullName} returned unsupported TypeCode.{typeCode.ToString()}");
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

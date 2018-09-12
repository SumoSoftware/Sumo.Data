using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Sumo.Data.Sqlite
{
    public static class TypeExtensions
    {
        public static SqliteType ToSqliteType(this DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Guid:
                case DbType.AnsiString: 
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                    return SqliteType.Text;

                case DbType.Binary:
                case DbType.Object:
                    return SqliteType.Blob;

                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.Time:
                case DbType.Boolean:
                case DbType.Byte:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.Single:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                    return SqliteType.Integer;

                case DbType.Currency:
                case DbType.Decimal:
                case DbType.Double:
                    return SqliteType.Real;

                case DbType.VarNumeric:
                default:
                    throw new NotSupportedException(dbType.ToString());
            }
        }

        public static SqliteType ToSqliteType(this Type type)
        {
            return type
                .ToDbType()
                .ToSqliteType();
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
    }
}

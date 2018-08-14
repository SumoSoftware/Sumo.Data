using System;
using System.Data;

namespace Sumo.Data.Schema.SqlServer
{
    public static class TypeExtensions
    {
        public static SqlDbType ToSqlDbType(this Type type)
        {
            return type
                .ToDbType()
                .ToSqlDbType();
        }

        public static DbType ToDbType(this SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.Udt:
                case SqlDbType.VarBinary: return DbType.Binary;
                case SqlDbType.BigInt: return DbType.Int64;
                case SqlDbType.Bit: return DbType.Boolean;
                case SqlDbType.Char: return DbType.AnsiStringFixedLength;
                case SqlDbType.Time: return DbType.Time;
                case SqlDbType.Date:
                case SqlDbType.DateTime: return DbType.DateTime;
                case SqlDbType.DateTime2: return DbType.DateTime2;
                case SqlDbType.DateTimeOffset: return DbType.DateTimeOffset;
                case SqlDbType.Decimal: return DbType.Decimal;
                case SqlDbType.Float: return DbType.Double;
                case SqlDbType.Int: return DbType.Int32;
                case SqlDbType.SmallMoney:
                case SqlDbType.Money: return DbType.Currency;
                case SqlDbType.NChar: return DbType.StringFixedLength;
                case SqlDbType.Text:
                case SqlDbType.NText:
                case SqlDbType.NVarChar: return DbType.String;
                case SqlDbType.Real: return DbType.Single;
                case SqlDbType.SmallDateTime:
                case SqlDbType.SmallInt: return DbType.Int16;
                case SqlDbType.TinyInt: return DbType.Byte;
                case SqlDbType.UniqueIdentifier: return DbType.Guid;
                case SqlDbType.VarChar: return DbType.AnsiString;
                case SqlDbType.Variant: return DbType.Object;
                case SqlDbType.Xml: return DbType.Xml;
                case SqlDbType.Structured:
                default:
                    throw new NotSupportedException(sqlDbType.ToString());
            }
        }


        public static SqlDbType ToSqlDbType(this DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString: return SqlDbType.VarChar;
                case DbType.AnsiStringFixedLength: return SqlDbType.Char;
                case DbType.Binary: return SqlDbType.VarBinary;
                case DbType.Boolean: return SqlDbType.Bit;
                case DbType.Byte: return SqlDbType.TinyInt;
                case DbType.Currency: return SqlDbType.Money;
                case DbType.Date:
                case DbType.DateTime: return SqlDbType.DateTime;
                case DbType.DateTime2: return SqlDbType.DateTime2;
                case DbType.DateTimeOffset: return SqlDbType.DateTimeOffset;
                case DbType.Decimal: return SqlDbType.Decimal;
                case DbType.Double: return SqlDbType.Float;
                case DbType.Guid: return SqlDbType.UniqueIdentifier;
                case DbType.Int16: return SqlDbType.SmallInt;
                case DbType.Int32: return SqlDbType.Int;
                case DbType.Int64: return SqlDbType.BigInt;
                case DbType.Object: return SqlDbType.Variant;
                case DbType.Single: return SqlDbType.Real;
                case DbType.String: return SqlDbType.NVarChar;
                case DbType.StringFixedLength: return SqlDbType.NChar;
                case DbType.Time: return SqlDbType.Time;
                case DbType.Xml: return SqlDbType.Xml;
                case DbType.SByte: // unsupported
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                default:
                    throw new NotSupportedException(dbType.ToString());
            }
        }

        //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
        public static Type ToType(this SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml: return typeof(String);
                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                case SqlDbType.Udt: return typeof(Byte[]);
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2: return typeof(DateTime);
                case SqlDbType.DateTimeOffset: return typeof(DateTimeOffset);
                case SqlDbType.Time: return typeof(TimeSpan);
                case SqlDbType.BigInt: return typeof(Int64);
                case SqlDbType.Bit: return typeof(Boolean);
                case SqlDbType.Float: return typeof(Double);
                case SqlDbType.Int: return typeof(Int32);
                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney: return typeof(Decimal);
                case SqlDbType.Real: return typeof(Single);
                case SqlDbType.SmallDateTime: return typeof(DateTime);
                case SqlDbType.SmallInt: return typeof(Int16);
                case SqlDbType.UniqueIdentifier: return typeof(Guid);
                case SqlDbType.TinyInt: return typeof(Byte);
                case SqlDbType.Variant: return typeof(Object);
                case SqlDbType.Structured:
                default:
                    throw new NotSupportedException(sqlDbType.ToString());
            }
        }
    }
}

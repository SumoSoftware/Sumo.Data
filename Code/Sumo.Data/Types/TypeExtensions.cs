using System;
using System.Data;

namespace Sumo.Data.Types
{
    public static class TypeExtensions
    {
        public static bool IsSizableType(this DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                    return true;
                default:
                    return false;
            }
        }

        public static DbType ToDbType(this Type type)
        {
            var code = Type.GetTypeCode(type);
            switch (code)
            {
                case TypeCode.Object:
                    if (type == typeof(Byte[]))
                    {
                        return DbType.Binary;
                    }
                    else if (type == typeof(Char[]))
                    {
                        return DbType.String;
                    }
                    else if (type == typeof(Guid))
                    {
                        return DbType.Guid;
                    }
                    else if (type == typeof(Object))
                    {
                        return DbType.Object;
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        return DbType.Time;
                    }
                    throw new NotSupportedException(type.FullName);
                case TypeCode.Boolean: return DbType.Boolean;
                case TypeCode.Byte:
                case TypeCode.Char: return DbType.Byte;
                case TypeCode.DateTime: return DbType.DateTime;
                case TypeCode.Decimal: return DbType.Decimal;
                case TypeCode.Double: return DbType.Double;
                case TypeCode.Int16: return DbType.Int16;
                case TypeCode.Int32: return DbType.Int32;
                case TypeCode.Int64: return DbType.Int64;
                case TypeCode.SByte: return DbType.SByte;
                case TypeCode.Single: return DbType.Single;
                case TypeCode.String: return DbType.String;
                case TypeCode.UInt16: return DbType.UInt16;
                case TypeCode.UInt32: return DbType.UInt32;
                case TypeCode.UInt64: return DbType.UInt64;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    throw new NotSupportedException($"{type.FullName} returned unsupported TypeCode.{code.ToString()}");
            }
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

        public static DbType ToDbType(this string typeName)
        {
            if (!Enum.TryParse(typeName, true, out DbType dbType)) throw new ArgumentOutOfRangeException($"{nameof(typeName)}:{typeName}");
            return dbType;
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.data.dbtype?view=netframework-4.7.2
        //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
        public static Type ToType(this DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.AnsiString:
                case DbType.StringFixedLength:
                case DbType.Xml: return typeof(String);
                case DbType.Binary: return typeof(Byte[]);
                case DbType.Boolean: return typeof(Boolean);
                case DbType.Byte: return typeof(Byte);
                case DbType.Time: return typeof(TimeSpan);
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2: return typeof(DateTime);
                case DbType.DateTimeOffset: return typeof(DateTimeOffset);
                case DbType.Currency:
                case DbType.VarNumeric:
                case DbType.Decimal: return typeof(Decimal);
                case DbType.Double: return typeof(Double);
                case DbType.Guid: return typeof(Guid);
                case DbType.Int16: return typeof(Int16);
                case DbType.Int32: return typeof(Int32);
                case DbType.Int64: return typeof(Int64);
                case DbType.Object: return typeof(Object);
                case DbType.SByte: return typeof(SByte);
                case DbType.Single: return typeof(Single);
                case DbType.UInt16: return typeof(UInt16);
                case DbType.UInt32: return typeof(UInt32);
                case DbType.UInt64: return typeof(UInt64);
                default:
                    throw new NotSupportedException(dbType.ToString());
            }
        }

        public static SqlDbType ToSqlDbType(this Type type)
        {
            return type
                .ToDbType()
                .ToSqlDbType();
        }

        public static SqlDbType ToSqlDbType(this string typeName)
        {
            if (!Enum.TryParse(typeName, true, out SqlDbType sqlDbType)) throw new ArgumentOutOfRangeException($"{nameof(typeName)}:{typeName}");
            return sqlDbType;
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

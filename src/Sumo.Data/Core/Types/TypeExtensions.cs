using System;
using System.Data;
using System.IO;

namespace Sumo.Data
{
    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

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
            var code = Type.GetTypeCode(type.IsNullable() ? Nullable.GetUnderlyingType(type) : type);
            switch (code)
            {
                case TypeCode.Object:
                    if (type == typeof(byte[]))
                    {
                        return DbType.Binary;
                    }
                    else if (type == typeof(char[]))
                    {
                        return DbType.String;
                    }
                    else if (type == typeof(Guid))
                    {
                        return DbType.Guid;
                    }
                    else if (type == typeof(object))
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
                case DbType.Xml: return typeof(string);
                case DbType.Binary: return typeof(byte[]);
                case DbType.Boolean: return typeof(bool);
                case DbType.Byte: return typeof(byte);
                case DbType.Time: return typeof(TimeSpan);
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2: return typeof(DateTime);
                case DbType.DateTimeOffset: return typeof(DateTimeOffset);
                case DbType.Currency:
                case DbType.VarNumeric:
                case DbType.Decimal: return typeof(decimal);
                case DbType.Double: return typeof(double);
                case DbType.Guid: return typeof(Guid);
                case DbType.Int16: return typeof(short);
                case DbType.Int32: return typeof(int);
                case DbType.Int64: return typeof(long);
                case DbType.Object: return typeof(object);
                case DbType.SByte: return typeof(sbyte);
                case DbType.Single: return typeof(float);
                case DbType.UInt16: return typeof(ushort);
                case DbType.UInt32: return typeof(uint);
                case DbType.UInt64: return typeof(ulong);
                default:
                    throw new NotSupportedException(dbType.ToString());
            }
        }

        //todo: move read and write to ORM namespace
        public static object Read(this BinaryReader reader, DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.AnsiString:
                case DbType.StringFixedLength: return reader.ReadString();
                case DbType.Boolean: return reader.ReadBoolean();
                case DbType.Byte: return reader.ReadByte();
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                    {
                        var ticks = reader.ReadInt64();
                        return new DateTime(ticks, DateTimeKind.Utc);
                    }
                case DbType.DateTimeOffset:
                    {
                        var offset = reader.ReadInt64();
                        var ticks = reader.ReadInt64();
                        return new DateTimeOffset(ticks, new TimeSpan(offset));
                    }
                case DbType.Currency:
                case DbType.VarNumeric:
                case DbType.Decimal: return reader.ReadDecimal();
                case DbType.Double: return reader.ReadDouble(); 
                case DbType.Guid: return new Guid(reader.ReadString());
                case DbType.Int16: return reader.ReadUInt16();
                case DbType.Int32: return reader.ReadInt32();
                case DbType.Int64: return reader.ReadInt64();
                case DbType.SByte: return reader.ReadSByte();
                case DbType.Single: return reader.ReadSingle();
                case DbType.UInt16: return reader.ReadUInt16();
                case DbType.UInt32: return reader.ReadUInt32();
                case DbType.UInt64: return reader.ReadUInt64();
                case DbType.Object:
                case DbType.Time:
                case DbType.Binary:
                case DbType.Xml:
                default:
                    throw new NotSupportedException(dbType.ToString());
            }

            throw new NotSupportedException(dbType.ToString());
        }

        public static void Write(this object value, BinaryWriter writer, DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.AnsiString:
                case DbType.StringFixedLength: writer.Write((string)value); break;
                case DbType.Boolean: writer.Write((bool)value); break;
                case DbType.Byte: writer.Write((byte)value); break;
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                    writer.Write(((DateTime)value).ToUniversalTime().Ticks);
                    break;
                case DbType.DateTimeOffset:
                    writer.Write(((DateTimeOffset)value).Offset.Ticks);
                    writer.Write(((DateTimeOffset)value).UtcTicks);
                    break;
                case DbType.Currency:
                case DbType.VarNumeric:
                case DbType.Decimal: writer.Write((decimal)value); break;
                case DbType.Double: writer.Write((double)value); break;
                case DbType.Guid: writer.Write(value.ToString()); break;
                case DbType.Int16: writer.Write((short)value); break;
                case DbType.Int32: writer.Write((int)value); break;
                case DbType.Int64: writer.Write((long)value); break;
                case DbType.SByte: writer.Write((sbyte)value); break;
                case DbType.Single: writer.Write((float)value); break;
                case DbType.UInt16: writer.Write((ushort)value); break;
                case DbType.UInt32: writer.Write((uint)value); break;
                case DbType.UInt64: writer.Write((ulong)value); break;
                case DbType.Object:
                case DbType.Time:
                case DbType.Binary:
                case DbType.Xml:
                default:
                    throw new NotSupportedException(dbType.ToString());
            }
        }
    }
}

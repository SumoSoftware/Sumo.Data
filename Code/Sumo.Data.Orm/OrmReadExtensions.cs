using Sumo.Data.Types;
using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace Sumo.Data.Orm
{
    public static class OrmReadExtensions
    {
        public static T ToObject<T>(this DataRow row) where T : class
        {
            // allows activator to use non-public constructors
            var result = (T)Activator.CreateInstance(
                typeof(T),
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public,
                null, null, null);
            //var result = Activator.CreateInstance<T>();
            for (var i = 0; i < TypeInfoCache<T>.Properties.Length; ++i)
            {
                var property = TypeInfoCache<T>.Properties[i];
                if (!row.IsNull(property.Name))
                {
                    property.SetValue(result, row[property.Name]);
                }
            }
            return result;
        }

        // for string, datetime, and numeric types
        public static object[] ToObjectArray(this DataRowCollection rows)
        {
            var result = new object[rows.Count];
            for (var i = 0; i < rows.Count; ++i)
            {
                result[i] = rows[i][0];
            }
            return result;
        }

        public static I[] ToArray<I, C>(this DataRowCollection rows) where C : class, I
        {
            var result = new I[rows.Count];
            for (var i = 0; i < rows.Count; ++i)
            {
                result[i] = rows[i].ToObject<C>();
            }
            return result;
        }

        public static T[] ToArray<T>(this DataRowCollection rows) where T : class
        {
            var result = new T[rows.Count];
            for (var i = 0; i < rows.Count; ++i)
            {
                result[i] = rows[i].ToObject<T>();
            }
            return result;
        }

        public static async Task<T[]> ToArrayAsync<T>(this DataRowCollection rows) where T : class
        {
            return await Task.Run(() =>
            {
                var result = new T[rows.Count];
                for (var i = 0; i < rows.Count; ++i)
                {
                    result[i] = rows[i].ToObject<T>();
                }
                return result;
            });
        }

        public static T[] ToArrayParallel<T>(this DataRowCollection rows) where T : class
        {
            var result = new T[rows.Count];
            Parallel.For(0, rows.Count, i =>
            {
                result[i] = rows[i].ToObject<T>();
            });
            return result;
        }

        //public static TypeCode ToTypeCode(this DbType dbType)
        //{
        //    switch (dbType)
        //    {
        //        case DbType.AnsiString:
        //        case DbType.AnsiStringFixedLength:
        //        case DbType.String:
        //        case DbType.StringFixedLength:
        //            return TypeCode.String;
        //        case DbType.Boolean:
        //            return TypeCode.Boolean;
        //        case DbType.Byte:
        //            return TypeCode.Byte;
        //        case DbType.VarNumeric:     // ???
        //        case DbType.Currency:
        //        case DbType.Decimal:
        //            return TypeCode.Decimal;
        //        case DbType.Date:
        //        case DbType.DateTime:
        //        case DbType.DateTime2: // new Katmai type
        //        case DbType.Time:      // new Katmai type - no TypeCode for TimeSpan
        //            return TypeCode.DateTime;
        //        case DbType.Double:
        //            return TypeCode.Double;
        //        case DbType.Int16:
        //            return TypeCode.Int16;
        //        case DbType.Int32:
        //            return TypeCode.Int32;
        //        case DbType.Int64:
        //            return TypeCode.Int64;
        //        case DbType.SByte:
        //            return TypeCode.SByte;
        //        case DbType.Single:
        //            return TypeCode.Single;
        //        case DbType.UInt16:
        //            return TypeCode.UInt16;
        //        case DbType.UInt32:
        //            return TypeCode.UInt32;
        //        case DbType.UInt64:
        //            return TypeCode.UInt64;
        //        case DbType.Guid:           // ???
        //        case DbType.Binary:
        //        case DbType.Object:
        //        case DbType.DateTimeOffset: // new Katmai type - no TypeCode for DateTimeOffset
        //        default:
        //            return TypeCode.Object;
        //    }
        //}

        //public static DbType ToDbType(this TypeCode typeCode)
        //{
        //    // no TypeCode equivalent for TimeSpan or DateTimeOffset
        //    switch (typeCode)
        //    {
        //        case TypeCode.Boolean:
        //            return DbType.Boolean;
        //        case TypeCode.Byte:
        //            return DbType.Byte;
        //        case TypeCode.Char:
        //            return DbType.StringFixedLength;    // ???
        //        case TypeCode.DateTime: // Used for Date, DateTime and DateTime2 DbTypes
        //            return DbType.DateTime;
        //        case TypeCode.Decimal:
        //            return DbType.Decimal;
        //        case TypeCode.Double:
        //            return DbType.Double;
        //        case TypeCode.Int16:
        //            return DbType.Int16;
        //        case TypeCode.Int32:
        //            return DbType.Int32;
        //        case TypeCode.Int64:
        //            return DbType.Int64;
        //        case TypeCode.SByte:
        //            return DbType.SByte;
        //        case TypeCode.Single:
        //            return DbType.Single;
        //        case TypeCode.String:
        //            return DbType.String;
        //        case TypeCode.UInt16:
        //            return DbType.UInt16;
        //        case TypeCode.UInt32:
        //            return DbType.UInt32;
        //        case TypeCode.UInt64:
        //            return DbType.UInt64;
        //        case TypeCode.DBNull:
        //        case TypeCode.Empty:
        //        case TypeCode.Object:
        //        default:
        //            return DbType.Object;
        //    }
        //}

        //public static DbType ToDbType(this Type type)
        //{
        //    if (type.IsArray)
        //    {
        //        if (type == typeof(byte[]))
        //        {
        //            return DbType.Binary;
        //        }
        //        else if (type == typeof(char[]))
        //        {
        //            return DbType.String;
        //        }
        //        else
        //        {
        //            return Type.GetTypeCode(type.GetElementType()).ToDbType();
        //        }
        //    }
        //    else
        //    {
        //        return Type.GetTypeCode(type).ToDbType();
        //    }
        //}

        //public static Type ToType(this TypeCode typeCode)
        //{
        //    switch (typeCode)
        //    {
        //        case TypeCode.Boolean:
        //            return typeof(bool);
        //        case TypeCode.Byte:
        //            return typeof(byte);
        //        case TypeCode.Char:
        //            return typeof(char);
        //        case TypeCode.DateTime:
        //            return typeof(DateTime);
        //        case TypeCode.Decimal:
        //            return typeof(decimal);
        //        case TypeCode.Double:
        //            return typeof(double);
        //        case TypeCode.Int16:
        //            return typeof(Int16);
        //        case TypeCode.Int32:
        //            return typeof(Int32);
        //        case TypeCode.Int64:
        //            return typeof(Int64);
        //        case TypeCode.SByte:
        //            return typeof(sbyte);
        //        case TypeCode.Single:
        //            return typeof(Single);
        //        case TypeCode.String:
        //            return typeof(string);
        //        case TypeCode.UInt16:
        //            return typeof(UInt16);
        //        case TypeCode.UInt32:
        //            return typeof(UInt32);
        //        case TypeCode.UInt64:
        //            return typeof(UInt64);
        //        case TypeCode.DBNull:
        //            return typeof(DBNull);
        //        case TypeCode.Empty:
        //            return null;
        //        case TypeCode.Object:
        //        default:
        //            return typeof(object);
        //    }
        //}
    }
}

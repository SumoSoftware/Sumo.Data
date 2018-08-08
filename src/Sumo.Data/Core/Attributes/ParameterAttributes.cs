using System;

namespace Sumo.Data
{
    /// <summary>
    /// Sql Type     | DbType                | Size | Returned string.Length()
    /// ----------------------------------------------------------------
    /// Varchar(10)  | AnsiString            | 10   | 9
    /// Char(10)     | AnsiStringFixedLength | 10   | 10
    /// Nvarchar(10  | String                | 10   | 9
    /// Varchar(max) | AnsiString            | -1   | 20,480 
    /// NVarchar(max)| String                | -1   | 20,480
    /// numeric types| ...                   |  0   | ...
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ParameterSizeAttribute : DataAttribute
    {
        public ParameterSizeAttribute() { }

        public ParameterSizeAttribute(int parameterSize)
        {
            ParameterSize = parameterSize;
        }

        //public ParameterSizeAttribute(SqlDbType type)
        //{
        //    throw new NotImplementedException();

        //    //todo: set size based on SqlDbType
        //    //switch (type)
        //    //{
        //    //    case SqlDbType.BigInt:
        //    //        break;
        //    //    case SqlDbType.Binary:
        //    //        break;
        //    //    case SqlDbType.Bit:
        //    //        break;
        //    //    case SqlDbType.Char:
        //    //        break;
        //    //    case SqlDbType.Date:
        //    //        break;
        //    //    case SqlDbType.DateTime:
        //    //        break;
        //    //    case SqlDbType.DateTime2:
        //    //        break;
        //    //    case SqlDbType.DateTimeOffset:
        //    //        break;
        //    //    case SqlDbType.Decimal:
        //    //        break;
        //    //    case SqlDbType.Float:
        //    //        break;
        //    //    case SqlDbType.Image:
        //    //        break;
        //    //    case SqlDbType.Int:
        //    //        break;
        //    //    case SqlDbType.Money:
        //    //        break;
        //    //    case SqlDbType.NChar:
        //    //        break;
        //    //    case SqlDbType.NText:
        //    //        break;
        //    //    case SqlDbType.NVarChar:
        //    //        break;
        //    //    case SqlDbType.Real:
        //    //        break;
        //    //    case SqlDbType.SmallDateTime:
        //    //        break;
        //    //    case SqlDbType.SmallInt:
        //    //        break;
        //    //    case SqlDbType.SmallMoney:
        //    //        break;
        //    //    case SqlDbType.Structured:
        //    //        break;
        //    //    case SqlDbType.Text:
        //    //        break;
        //    //    case SqlDbType.Time:
        //    //        break;
        //    //    case SqlDbType.Timestamp:
        //    //        break;
        //    //    case SqlDbType.TinyInt:
        //    //        break;
        //    //    case SqlDbType.Udt:
        //    //        break;
        //    //    case SqlDbType.UniqueIdentifier:
        //    //        break;
        //    //    case SqlDbType.VarBinary:
        //    //        break;
        //    //    case SqlDbType.VarChar:
        //    //        break;
        //    //    case SqlDbType.Variant:
        //    //        break;
        //    //    case SqlDbType.Xml:
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}
        //}

        public int ParameterSize { get; } = -1;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InputOutputParameterAttribute : ParameterSizeAttribute
    {
        public InputOutputParameterAttribute() : base() { }

        public InputOutputParameterAttribute(int parameterSize) : base(parameterSize) { }

        //public InputOutputParameterAttribute(SqlDbType type) : base(type) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OutputParameterAttribute : ParameterSizeAttribute
    {
        public OutputParameterAttribute() : base() { }

        public OutputParameterAttribute(int parameterSize) : base(parameterSize) { }

        //public OutputParameterAttribute(SqlDbType type) : base(type) { }
    }
}

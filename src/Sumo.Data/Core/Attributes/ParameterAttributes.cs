using System;

namespace Sumo.Data
{
    /// <summary>
    /// Sql Type     | DbType                | Size | Returned String.Length()
    /// ----------------------------------------------------------------
    /// Varchar(10)  | Ansistring            | 10   | 9
    /// Char(10)     | AnsiStringFixedLength | 10   | 10
    /// Nvarchar(10  | string                | 10   | 9
    /// Varchar(max) | Ansistring            | -1   | 20,480 
    /// NVarchar(max)| string                | -1   | 20,480
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

        public int ParameterSize { get; } = -1;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InputOutputParameterAttribute : ParameterSizeAttribute
    {
        public InputOutputParameterAttribute() : base() { }

        public InputOutputParameterAttribute(int parameterSize) : base(parameterSize) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OutputParameterAttribute : ParameterSizeAttribute
    {
        public OutputParameterAttribute() : base() { }

        public OutputParameterAttribute(int parameterSize) : base(parameterSize) { }
    }
}

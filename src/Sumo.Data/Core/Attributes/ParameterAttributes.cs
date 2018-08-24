using System;

namespace Sumo.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ParameterNameAttribute : DataAttribute
    {
        public ParameterNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

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
    public class InputParameterAttribute : ParameterNameAttribute
    {
        public InputParameterAttribute() : base(string.Empty) { }

        public InputParameterAttribute(string name) : base(name) { }

        public InputParameterAttribute(int parameterSize) : base(string.Empty)
        {
            ParameterSize = parameterSize;
        }

        public InputParameterAttribute(string name, int parameterSize) : base(name)
        {
            ParameterSize = parameterSize;
        }

        public int ParameterSize { get; } = -1;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InputOutputParameterAttribute : InputParameterAttribute
    {
        public InputOutputParameterAttribute() : base(string.Empty) { }

        public InputOutputParameterAttribute(string name) : base(name) { }

        public InputOutputParameterAttribute(string name, int parameterSize) : base(name, parameterSize) { }

        public InputOutputParameterAttribute(int parameterSize) : base(parameterSize) { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OutputParameterAttribute : InputParameterAttribute
    {
        public OutputParameterAttribute() : base(string.Empty) { }

        public OutputParameterAttribute(string name) : base(name) { }

        public OutputParameterAttribute(string name, int parameterSize) : base(name, parameterSize) { }

        public OutputParameterAttribute(int parameterSize) : base(parameterSize) { }
    }
}

using System;
using System.Data;

namespace Sumo.Data.Datasets
{
    public class Field
    {
        public Field() : base() { }

        public Field(string name) : base()
        {
            Name = name;
        }

        public Field(string name, int ordinalPosition) : this(name)
        {
            OrdinalPosition = ordinalPosition;
        }

        public Field(string name, TypeCode type) : this(name)
        {
            TypeCode = type;
        }

        public Field(string name, TypeCode type, int ordinalPosition) : this(name, type)
        {
            OrdinalPosition = ordinalPosition;
        }

        public Field(string name, TypeCode type, int ordinalPosition, object defaultValue) : this(name, type, ordinalPosition)
        {
            Default = defaultValue;
        }

        public Field(string name, Type type, int ordinalPosition) : this(name, Type.GetTypeCode(type), ordinalPosition)
        {
        }

        public Field(DataColumn column) : this(column.ColumnName, column.DataType, column.Ordinal)
        {
            if (TypeCode == TypeCode.Empty) throw new ArgumentException($"{nameof(column)}.{column.ColumnName} data type is invalid.");
            if (TypeCode == TypeCode.Object) TypeName = column.DataType.FullName;
        }

        public string Name { get; set; }
        public TypeCode TypeCode { get; set; }
        public string TypeName { get; set; }
        public int OrdinalPosition { get; set; } = 0;
        public object Default { get; set; } = null;

        public override string ToString()
        {
            return $"[{Name}]:{TypeCode}{(TypeCode == TypeCode.Object ? ":" + TypeName : string.Empty)}:{OrdinalPosition}";
        }
    }
}

using System;
using System.Data;

namespace Sumo.Data
{
    /// <summary>
    /// Field definitions, similar to System.Data.DataColumn
    /// </summary>
    public class Field
    {
        public Field() : base() { }

        private Field(string name) : base()
        {
            Name = name;
        }

        private Field(string name, TypeCode typeCode) : this(name)
        {
            TypeCode = typeCode;
        }

        public Field(string name, int ordinalPosition) : this(name)
        {
            OrdinalPosition = ordinalPosition;
        }

        public Field(string name, TypeCode typeCode, int ordinalPosition) : this(name, typeCode)
        {
            OrdinalPosition = ordinalPosition;
        }

        public Field(string name, TypeCode typeCode, int ordinalPosition, object defaultValue) : this(name, typeCode, ordinalPosition)
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
        /// <summary>
        /// OrdinalPosition is used by the ORM to map fields defintions to record field indexes by name
        /// </summary>
        public int OrdinalPosition { get; set; } = 0;
        public object Default { get; set; } = null;

        public override string ToString()
        {
            return $"[{Name}] P: {OrdinalPosition} T: {TypeCode}{(TypeCode == TypeCode.Object ? ":" + TypeName : string.Empty)}";
        }
    }
}

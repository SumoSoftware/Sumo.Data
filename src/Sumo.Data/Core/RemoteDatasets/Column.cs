using System;
using System.Data;

namespace Sumo.Data.Core.Datasets
{
    public class Column
    {
        public Column() : base() { }

        public Column(string name) : base()
        {
            Name = name;
        }

        public Column(string name, TypeCode type) : this(name)
        {
            TypeCode = type;
        }

        public Column(string name, TypeCode type, int ordinalPosition) : this(name, type)
        {
            OrdinalPosition = ordinalPosition;
        }

        public Column(string name, TypeCode type, int ordinalPosition, object defaultValue) : this(name, type, ordinalPosition)
        {
            Default = defaultValue;
        }

        public Column(DataColumn column) : this(column.ColumnName, Type.GetTypeCode(column.DataType), column.Ordinal)
        {
            if (TypeCode == TypeCode.Empty) throw new ArgumentException($"{nameof(column)}.{column.ColumnName} data type is invalid.");
            if(TypeCode == TypeCode.Object) TypeName = column.DataType.FullName;
        }

        public string Name { get; set; }
        public TypeCode TypeCode { get; set; }
        public string TypeName { get; set; }
        public int OrdinalPosition { get; set; } = 0;
        public object Default { get; set; } = null;
    }
}

using System;
using System.Reflection;

namespace Sumo.Csv
{
    public class CsvColumn
    {
        /// <summary>
        /// OrdinalPosition is a zero based index indicating the order of the column as it appears in the csv file
        /// </summary>
        public int OrdinalPosition { get; set; }

        /// <summary>
        /// Name of the coresponding property that the value will be assigned to on the importing type.
        /// Example
        /// 
        /// For a class such as:
        /// 
        /// class MyDemo
        ///   Public Property  MyProperty as String
        /// end class
        /// 
        /// public class MyDemo
        /// {
        ///     public string MyProperty{ get; set; }
        /// }
        /// 
        /// you must have at least one column with a name of "MyProperty"
        /// 
        /// </summary>
        /// <remarks>csv column names and class property names must match exactly.</remarks>
        public string PropertyName { get; set; }

        /// <summary>
        /// if the properties are provided ahead of time during the creation of the columns, the performance is much better
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// For Read operations only. Provides a default value if the CSV doesn't contain a value for this column.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Provides a means of validating column input data during Read operations.
        /// </summary>
        public string ValidationRegExPattern { get; set; }

        public override string ToString()
        {
            var name = PropertyName;
            if (Property != null)
                name = Property.Name;
            return string.Format("{0} - {1}", OrdinalPosition.ToString(), name);
        }
    }

    public abstract class ArrayColumn : CsvColumn
    {
        internal abstract dynamic CreateArray(int outerArrayLength);
        internal abstract void SetValue<T>(T value);
        internal abstract Type GetCurrentType();
        internal abstract bool Increment();
        protected abstract void Reset();

        protected dynamic Values { get; set; } = null;
    }

    public sealed class OneDimensionalArrayColumn : ArrayColumn
    {
        public OneDimensionalArrayColumn(Type arrayType) : base()
        {
            _arrayType = arrayType;
        }

        private readonly Type _arrayType;
        private int _arrayLength = 0;
        private int _index = 0;

        internal override bool Increment()
        {
            var result = ++_index < _arrayLength;
            if (!result)
                Reset();
            return result;
        }

        protected override void Reset()
        {
            _index = 0;
            _arrayLength = 0;
            Values = null;
        }

        internal override dynamic CreateArray(int outerArrayLength)
        {
            Values = Array.CreateInstance(_arrayType, outerArrayLength);
            _arrayLength = outerArrayLength;
            return Values;
        }

        internal override void SetValue<T>(T value)
        {
            Values[_index] = value;
        }

        internal override Type GetCurrentType()
        {
            return _arrayType;
        }
    }

    public class TwoDimensionalArrayColumn : ArrayColumn
    {
        public TwoDimensionalArrayColumn(Type arrayType, int secondDimensionLength, Type[] types) : base()
        {
            _arrayType = arrayType;
            _secondDimensionLength = secondDimensionLength;
            _types = types;
        }

        private readonly int _secondDimensionLength;
        private readonly int[] _indices = new int[2];
        private readonly Type[] _types;
        private readonly Type _arrayType;
        private int _arrayLength = 0;

        internal override bool Increment()
        {
            ++_indices[1];
            if (_indices[1] == _secondDimensionLength)
            {
                _indices[1] = 0;
                ++_indices[0];
            }
            var result = _indices[0] < _arrayLength;
            if (!result)
                Reset();
            return result;
        }

        protected override void Reset()
        {
            _indices[0] = 0;
            _indices[1] = 0;
            _arrayLength = 0;
            Values = null;
        }

        internal override dynamic CreateArray(int outerArrayLength)
        {
            Values = Array.CreateInstance(_arrayType, outerArrayLength, _secondDimensionLength);
            _arrayLength = outerArrayLength;
            return Values;
        }

        internal override void SetValue<T>(T value)
        {
            (Values as Array).SetValue(value, _indices);
        }

        internal override Type GetCurrentType()
        {
            if (_types == null)
                return _arrayType;
            else
                return _types[_indices[1]];
        }
    }

    public class WriteCsvColumn : CsvColumn
    {
        /// <summary>
        /// Used during Write operations only.
        /// </summary>
        /// <remarks>Note: Using headings in CSV files violates the standard. Headings are included for debugging convenience, but use in production code is not recommended.</remarks>
        public string Heading { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} output to {2}", OrdinalPosition.ToString(), PropertyName, Heading);
        }

        /// <summary>
        /// Use to specify a string format for numeric and date/time types.
        /// </summary>
        /// <remarks>For a list of valid format specifiers, see http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx </remarks>
        public string FormatSpecifier { get; set; }

        private TypeCode _typeCode = TypeCode.Empty;
        public TypeCode TypeCode
        {
            get
            {
                if (_typeCode == TypeCode.Empty)
                    _typeCode = Type.GetTypeCode(Property.PropertyType);
                return _typeCode;
            }
        }
    }
}

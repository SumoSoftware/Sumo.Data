using System;
using System.Reflection;

namespace Sumo.Data.Csv
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

    //todo: consider moving typecode property to CsvColumn
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

using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sumo.Data.Csv
{
    //todo: consider merging the Recordset Field class and the CsvColumn class - this was the point of moving Csv to Sumo.Data
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

        private PropertyInfo _property = null;
        /// <summary>
        /// if the properties are provided ahead of time during the creation of the columns, the performance is much better
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get => _property;
            set
            {
                _property = value;
                if (_property != null && string.IsNullOrEmpty(PropertyName)) PropertyName = _property.Name;
            }
        }

        /// <summary>
        /// For Read operations only. Provides a default value if the CSV doesn't contain a value for this column.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Provides a means of validating column input data during Read operations.
        /// </summary>
        public string ValidationRegExPattern { get; set; }

        internal void ValidateValue(string value)
        {
            if (!string.IsNullOrEmpty(ValidationRegExPattern) && !Regex.IsMatch(value, ValidationRegExPattern))
            {
                throw new CsvException($"value: '{value}' in column {OrdinalPosition} failed validation.");
            }
        }

        private TypeCode _typeCode = TypeCode.Empty;
        public TypeCode TypeCode
        {
            get
            {
                if (_typeCode == TypeCode.Empty)
                {
                    var type = PropertyInfo.PropertyType.GenericTypeArguments.Length == 0 ? PropertyInfo.PropertyType : PropertyInfo.PropertyType.GenericTypeArguments[0];
                    _typeCode = Type.GetTypeCode(type);
                }
                return _typeCode;
            }
        }

        public override string ToString()
        {
            var name = PropertyName;
            return $"{OrdinalPosition}: {name}";
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
    }
}

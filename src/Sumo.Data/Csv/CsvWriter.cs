using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sumo.Data.Csv
{
    //todo: add support for meta data serialization (like the newtonsoft json type settings)
    public sealed class CsvWriter<T>
    {
        private WriteCsvColumn[] _columns;
        private Type _type;
        private PropertyInfo[] _properties;

        public CsvWriter(WriteCsvColumn[] columns)
        {
            _type = typeof(T);
            _properties = _type.GetProperties();
            _columns = columns.OrderBy(c => c.OrdinalPosition).ToArray();
        }

        public CsvWriter()
        {
            _type = typeof(T);
            _properties = _type.GetProperties();
            _columns = new WriteCsvColumn[_properties.Length];
            for (var i = 0; i < _properties.Length; ++i)
                _columns[i] = new WriteCsvColumn
                {
                    Property = _properties[i],
                    OrdinalPosition = i,
                    Heading = _properties[i].Name,
                    PropertyName = _properties[i].Name
                };
        }

        public string Write(IEnumerable<T> items, bool includeHeader = true)
        {
            var builder = new StringBuilder();

            var line = string.Empty;
            if (includeHeader)
            {
                for (var i = 0; i < _columns.Length; ++i)
                    builder.Append("\"" + _columns[i].Heading.Replace("\"", "\"\"") + "\"" + (i < _columns.Length - 1 ? "," : string.Empty));
                builder.Append(Environment.NewLine);
            }

            foreach (var item in items)
            {
                for (var i = 0; i < _columns.Length; ++i)
                {
                    var column = _columns[i];
                    var value = column.Property.GetValue(item);
                    var valueText = string.Empty;
                    if (value != null)
                    {
                        switch (column.TypeCode)
                        {
                            case TypeCode.Boolean:
                                valueText = ((bool)value).ToString();
                                break;
                            case TypeCode.Char:
                                valueText = ((char)value).ToString();
                                break;
                            case TypeCode.SByte:
                                valueText = ((sbyte)value).ToString();
                                break;
                            case TypeCode.Byte:
                                valueText = ((byte)value).ToString();
                                break;
                            case TypeCode.Int16:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((short)value).ToString() : ((short)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.UInt16:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((ushort)value).ToString() : ((ushort)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.Int32:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((int)value).ToString() : ((int)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.UInt32:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((uint)value).ToString() : ((uint)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.Int64:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((long)value).ToString() : ((long)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.UInt64:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((ulong)value).ToString() : ((ulong)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.Single:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((float)value).ToString() : ((float)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.Double:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((double)value).ToString() : ((double)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.Decimal:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((decimal)value).ToString() : ((decimal)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.DateTime:
                                valueText = string.IsNullOrEmpty(column.FormatSpecifier) ? ((DateTime)value).ToString() : ((DateTime)value).ToString(column.FormatSpecifier);
                                break;
                            case TypeCode.String:
                                valueText = (string)value;
                                if (string.IsNullOrEmpty(valueText))
                                    valueText = column.DefaultValue;
                                valueText = "\"" + valueText + "\"";
                                break;
                            case TypeCode.Object:
                                if (column.Property.PropertyType == typeof(byte[]))
                                    valueText = Convert.ToBase64String((byte[])value);
                                else if (column.Property.PropertyType == typeof(char[]))
                                    valueText = "invalid_type_char";
                                else
                                    valueText = value != null ? value.ToString() : column.DefaultValue;
                                break;
                            default:
                                valueText = value != null ? value.ToString() : column.DefaultValue;
                                break;
                        }
                    }
                    else
                    {
                        valueText = column.TypeCode != TypeCode.String ? column.DefaultValue : "\"" + column.DefaultValue + "\"";
                    }
                    builder.Append(valueText);
                    if (i < _columns.Length - 1)
                        builder.Append(",");
                }
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();
        }
    }

}

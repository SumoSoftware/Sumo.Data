using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sumo.Data.Csv
{
    public sealed class CsvReader<T> where T : class
    {
        private readonly CsvColumn[] _columns;
        private const string _csvRegexPattern = "^(?:\"(?<value>(?:\"\"|[^\"\n\f\r])*)\"|(?<value>[^,\n\f\r\"]*))(?:,(?:[ \t]*\"(?<value>(?:\"\"|[^\"\n\f\r])*)\"|(?<value>[^,\n\f\r\"]*)))*$";
        //,*[ \t]*\"(?<value>(?:\"\"|[^\"\n\f\r])*)\"|(?<value>[^,\n\f\r\"]*))
        //const string _csvRegexPattern = ",*[ \t]*\"(?<value>(?:\"\"|[^\"\n\f\r])*)\"|(?<value>[^,\n\f\r\"]*))";

        public CsvReader()
        {
            _columns = new CsvColumn[TypeInfoCache<T>.ReadWriteProperties.Length];
            for (var i = 0; i < _columns.Length; ++i)
            {
                _columns[i] = new CsvColumn()
                {
                    OrdinalPosition = i,
                    PropertyName = TypeInfoCache<T>.ReadWritePropertyNames[i],
                    PropertyInfo = TypeInfoCache<T>.ReadWriteProperties[i]
                };
            }
        }

        public CsvReader(CsvColumn[] columns) : base()
        {
            _columns = columns ?? throw new ArgumentNullException("columns");
            _columns = _columns.OrderBy(c => c.OrdinalPosition).ToArray();
            for (var i = 0; i < _columns.Length; ++i)
            {
                var column = _columns[i];
                if (column.PropertyInfo == null)
                {
                    if (string.IsNullOrEmpty(column.PropertyName))
                    {
                        throw new InvalidOperationException($"PropertyInfo or PropertyName must be set on column index {i}.");
                    }
                    column.PropertyInfo = TypeInfoCache<T>.GetReadWriteProperty(column.PropertyName);
                }
            }
        }

        public T[] Read(string csvContent, bool ignoreFirstLine = false)
        {
            if (_columns == null) throw new InvalidOperationException("This version of read requires that columns be passed to the constructor.");

            return Read(csvContent, _columns, ignoreFirstLine);
        }

        public T[] Read(string csvContent, CsvColumn[] columns, bool ignoreFirstLine = false)
        {
            if (csvContent == null) throw new ArgumentNullException("csvContent");
            if (columns == null) throw new ArgumentNullException("columns");

            var lines = csvContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var indexOffset = ignoreFirstLine ? 1 : 0;
            var result = new T[lines.Length - indexOffset];

            if (lines.Length > 0)
            {
                var startIndex = ignoreFirstLine ? 1 : 0;
                for (var i = startIndex; i < lines.Length; ++i)
                {
                    var line = lines[i];
                    try
                    {
                        if (line.Trim() != string.Empty)
                        {
                            var rowElement = LineToObject(line, columns);
                            if (rowElement != null)
                                result[i - indexOffset] = rowElement;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new CsvException(string.Format("{0} failed on line # {1}", GetType().Name, i), ex);
                    }
                }
            }
            return result;
        }

        public T ReadLine(string csvLine, T result = default(T))
        {
            if (csvLine == null) throw new ArgumentNullException("csvLine");

            return LineToObject(csvLine, _columns, result);
        }

        internal T LineToObject(string line, CsvColumn[] columns, T result = default(T))
        {
            if (line == null) throw new ArgumentNullException(nameof(line));
            if (columns == null) throw new ArgumentNullException(nameof(columns));

            var match = Regex.Match(line, _csvRegexPattern, RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                if (ReferenceEquals(result, default(T)))
                {
                    result = Activator.CreateInstance<T>();
                }

                var group = match.Groups["value"];
                for (int i = 0; i < columns.Length; ++i)
                {
                    var column = columns[i];
                    var value = GetStringValueFromCaptures(group.Captures, 0, column);
                    var typedValue = GetTypedValueFromString(value, column.PropertyInfo.PropertyType, column.TypeCode);
                    column.PropertyInfo.SetValue(result, typedValue);
                }
            }
            else
            {
                throw new CsvException($"csv line is not well formed: {line}");
            }
            return result;
        }

        internal static string GetStringValueFromCaptures(CaptureCollection captures, int positionOffset, CsvColumn column)
        {
            var value = captures[column.OrdinalPosition + positionOffset].Value;
            if (value != null)
            {
                // remove double quotes which are CSV escapes according to RFC 4180
                value = value.Replace("\"\"", "\"");
                if (value == string.Empty)
                {
                    value = column.DefaultValue;
                }
                else
                {
                    column.ValidateValue(value);
                }
            }
            else
            {
                value = column.DefaultValue;
            }
            return value;
        }

        internal static object GetTypedValueFromString(string value, Type type, TypeCode typeCode = TypeCode.Empty)
        {
            try
            {
                if(typeCode == TypeCode.Empty)
                {
                    type = type.GenericTypeArguments.Length == 0 ? type : type.GenericTypeArguments[0];
                    typeCode = Type.GetTypeCode(type);
                }

                switch (typeCode)
                {
                    case TypeCode.Object:
                        if (type == typeof(TimeSpan))
                        {
                            return TimeSpan.Parse(value);
                        }
                        else if (type == typeof(Guid))
                        {
                            return Guid.Parse(value);
                        }
                        else if (type == typeof(byte[]))
                        {
                            // assume base 64 encoded data
                            return Convert.FromBase64String(value);
                        }
                        else if (type == typeof(char[]))
                        {
                            return value.ToCharArray();
                        }
                        throw new NotSupportedException($"Type '{type.FullName}' with TypeCode {typeCode} not supported.");
                    case TypeCode.String: return value;
                    case TypeCode.Boolean: return bool.Parse(value);
                    case TypeCode.Char: return char.Parse(value);
                    case TypeCode.Byte: return byte.Parse(value);
                    case TypeCode.SByte: return sbyte.Parse(value);
                    case TypeCode.Decimal: return decimal.Parse(value);
                    case TypeCode.Double: return double.Parse(value);
                    case TypeCode.Single: return float.Parse(value);
                    case TypeCode.Int16: return short.Parse(value);
                    case TypeCode.Int32: return int.Parse(value);
                    case TypeCode.Int64: return long.Parse(value);
                    case TypeCode.UInt16: return ushort.Parse(value);
                    case TypeCode.UInt32: return uint.Parse(value);
                    case TypeCode.UInt64: return ulong.Parse(value);
                    case TypeCode.DateTime: return DateTime.Parse(value);

                    case TypeCode.DBNull:
                    case TypeCode.Empty:
                    default:
                        throw new NotSupportedException($"Type '{type.FullName}' with TypeCode {typeCode} not supported.");
                }
            }
            catch (Exception ex)
            {
                throw new CsvException($"{nameof(GetTypedValueFromString)} failed on value '{value}', with message: {ex.Message}. See inner exception for details.", ex);
            }
        }

        // an idea to iterate the string only one time by regex instead of once by split and then again on each substring with regex
        //var match = Regex.Match(csvContent, _csvRegexPattern, RegexOptions.ExplicitCapture);
        //var lineCount = 0;
        //var nextMatch = match;
        //while (nextMatch.Success)
        //{
        //    if (nextMatch.Length > 0)
        //        ++lineCount;
        //    nextMatch = match.NextMatch();
        //}

        //var result = new T[lineCount];
        //var lineIndex = 0;
        //long columnIndex = 0;
        //nextMatch = match;
        //T item = default(T);
        //while (nextMatch.Success)
        //{
        //    if (nextMatch.Length > 0)
        //    {
        //        if (columnIndex % _columns.Length == 0)
        //        {
        //            item = default(T);
        //            result[lineIndex++] = item;
        //        }
        //        var column = _columns[columnIndex++];
        //        PropertyInfo property = null;
        //        if (_propertyInfoMap.TryGetValue(column.PropertyName, out property))
        //        {
        //            var value = nextMatch.Value;
        //            if (value != null)
        //            {
        //                value = value.Replace("\"\"", "\"");
        //                if (value == string.Empty)
        //                    value = column.DefaultValue;
        //                if (!IsValueValid(value, column))
        //                    throw new CsvException(string.Format("value: '{0}' in column {1} failed validation.", value, column.Position));
        //                property.SetValue(item, GetTypedValue(value, property.PropertyType));
        //            }
        //        }
        //    }
        //}
        //return result;
    }
}



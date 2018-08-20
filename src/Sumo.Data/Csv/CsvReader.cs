using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sumo.Data.Csv
{
    public sealed class CsvReader<T>
    {
        private bool _columnsHaveProperties;
        private Dictionary<string, PropertyInfo> _propertyInfoMap;
        private readonly CsvColumn[] _columns;
        private const string _csvRegexPattern = "^(?:\"(?<value>(?:\"\"|[^\"\n\f\r])*)\"|(?<value>[^,\n\f\r\"]*))(?:,(?:[ \t]*\"(?<value>(?:\"\"|[^\"\n\f\r])*)\"|(?<value>[^,\n\f\r\"]*)))*$";
        //,*[ \t]*\"(?<value>(?:\"\"|[^\"\n\f\r])*)\"|(?<value>[^,\n\f\r\"]*))
        //const string _csvRegexPattern = ",*[ \t]*\"(?<value>(?:\"\"|[^\"\n\f\r])*)\"|(?<value>[^,\n\f\r\"]*))";

        public CsvReader(CsvColumn[] columns, bool columnsHaveProperties = false) : base()
        {
            _columns = columns ?? throw new ArgumentNullException("columns");
            Initialize(columnsHaveProperties);
        }

        public CsvReader(bool columnsHaveProperties = false) : base()
        {
            Initialize(columnsHaveProperties);
        }

        private void Initialize(bool columnsHaveProperties)
        {
            _columnsHaveProperties = columnsHaveProperties;
            if (!columnsHaveProperties)
            {
                var type = typeof(T);
                if (type == null)
                    throw new CsvException(string.Format("{0} failed in the constructor. typeof(T) was null.", GetType().Name));
                _propertyInfoMap = new Dictionary<string, PropertyInfo>();
                var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                for (var i = 0; i < propertyInfos.Length; i++)
                    _propertyInfoMap[propertyInfos[i].Name] = propertyInfos[i];
            }
        }

        public T[] Read(string csvContent, bool ignoreFirstLine = false)
        {
            if (csvContent == null) throw new ArgumentNullException("csvContent");

            var lines = csvContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var arrayModifier = 0;
            if (ignoreFirstLine) arrayModifier = 1;

            var result = new T[lines.Length - arrayModifier];

            if (lines.Length > 0)
            {
                var startIndex = ignoreFirstLine ? 1 : 0;
                if (!_columnsHaveProperties)
                {
                    for (var i = startIndex; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        try
                        {
                            if (line.Trim() != string.Empty)
                            {
                                var rowElement = GenerateRowElementUsingPropertyMap(line, _columns);
                                if (rowElement != null)
                                    result[i - arrayModifier] = rowElement;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new CsvException(string.Format("{0} failed on line # {1}", GetType().Name, i), ex);
                        }
                    }
                }
                else
                {
                    for (var i = startIndex; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        try
                        {
                            if (line.Trim() != string.Empty)
                            {
                                var rowElement = GenerateRowElementUsingColumnMap(line, _columns);
                                if (rowElement != null)
                                    result[i - arrayModifier] = rowElement;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new CsvException(string.Format("{0} failed on line # {1}", GetType().Name, i), ex);
                        }
                    }
                }
            }
            return result;
        }

        public T[] Read(string csvContent, CsvColumn[] columns, bool ignoreFirstLine = false)
        {
            if (csvContent == null) throw new ArgumentNullException("csvContent");
            if (columns == null) throw new ArgumentNullException("columns");

            var lines = csvContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var arrayModifier = 0;
            if (ignoreFirstLine) arrayModifier = 1;

            var result = new T[lines.Length - arrayModifier];

            if (lines.Length > 0)
            {
                var startIndex = ignoreFirstLine ? 1 : 0;
                if (!_columnsHaveProperties)
                {
                    for (var i = startIndex; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        try
                        {
                            if (line.Trim() != string.Empty)
                            {
                                var rowElement = GenerateRowElementUsingPropertyMap(line, columns);
                                if (rowElement != null)
                                    result[i - arrayModifier] = rowElement;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new CsvException(string.Format("{0} failed on line # {1}", GetType().Name, i), ex);
                        }
                    }
                }
                else
                {
                    for (var i = startIndex; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        try
                        {
                            if (line.Trim() != string.Empty)
                            {
                                var rowElement = GenerateRowElementUsingColumnMap(line, columns);
                                if (rowElement != null)
                                    result[i - arrayModifier] = rowElement;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new CsvException(string.Format("{0} failed on line # {1}", GetType().Name, i), ex);
                        }
                    }
                }
            }
            return result;
        }

        public T ReadLine(string csvLine, T result = default(T))
        {
            if (csvLine == null) throw new ArgumentNullException("csvLine");

            return GenerateRowElementUsingColumnMap(csvLine, _columns, result);
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

        private T GenerateRowElementUsingPropertyMap(string csvLine, CsvColumn[] columns)
        {
            if (csvLine == null) throw new ArgumentNullException("csvLine");
            if (columns == null) throw new ArgumentNullException("columns");

            T result = default(T);
            var match = Regex.Match(csvLine, _csvRegexPattern, RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                var group = match.Groups["value"];
                result = Activator.CreateInstance<T>();
                for (int i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    if (column != null)
                    {
                        if (_propertyInfoMap.TryGetValue(column.PropertyName, out PropertyInfo property))
                        {
                            var value = group.Captures[column.OrdinalPosition].Value;
                            if (value != null)
                            {
                                value = value.Replace("\"\"", "\"");
                                if (value == string.Empty)
                                    value = column.DefaultValue;
                                if (!IsValueValid(value, column))
                                    throw new CsvException(string.Format("value: '{0}' in column {1} failed validation.", value, column.OrdinalPosition));
                                property.SetValue(result, GetTypedValueFromString(value, property.PropertyType));
                            }
                        }
                    }
                }
            }
            else
                throw new CsvException(string.Format("csv line is not well formed: {0}", csvLine));
            return result;
        }

        private T GenerateRowElementUsingColumnMap(string csvLine, CsvColumn[] columns, T result = default(T))
        {
            if (csvLine == null) throw new ArgumentNullException("csvLine");
            if (columns == null) throw new ArgumentNullException("columns");

            var match = Regex.Match(csvLine, _csvRegexPattern, RegexOptions.ExplicitCapture);
            //int arrayLength = 0;
            int positionOffset = 0;

            if (match.Success)
            {
                var group = match.Groups["value"];

                if (ReferenceEquals(result, default(T)))
                    result = Activator.CreateInstance<T>();

                for (int i = 0; i < columns.Length; ++i)
                {
                    var column = columns[i];
                    if (column != null && column.Property != null)
                    {
                        var value = GetStringValueFromCaptures(group.Captures, positionOffset, column);

                        //if (!(column is ArrayColumn))
                        //{
                        column.Property.SetValue(result, GetTypedValueFromString(value, column.Property.PropertyType));
                        //}
                        //else
                        //{
                        //    if (!column.Property.PropertyType.IsArray)
                        //        throw new CsvException($"{column.Property.PropertyType.Name} must be an array.");

                        //    arrayLength = (int)GetTypedValueFromString(value, typeof(int));
                        //    var col = (ArrayColumn)column;
                        //    column.Property.SetValue(result, col.CreateArray(arrayLength));
                        //    do
                        //    {
                        //        ++positionOffset;
                        //        var arrayValue = GetStringValueFromCaptures(group.Captures, positionOffset, column);
                        //        col.SetValue(GetTypedValueFromString(arrayValue, col.GetCurrentType()));
                        //    } while (col.Increment());
                        //}
                    }
                }
            }
            else
                throw new CsvException(string.Format("csv line is not well formed: {0}", csvLine));
            return result;
        }

        private string GetStringValueFromCaptures(CaptureCollection captures, int positionOffset, CsvColumn column)
        {
            var value = captures[column.OrdinalPosition + positionOffset].Value;
            if (value != null)
            {
                value = value.Replace("\"\"", "\"");

                if (value == string.Empty)
                    value = column.DefaultValue;

                if (!IsValueValid(value, column))
                    throw new CsvException(string.Format("value: '{0}' in column {1} failed validation.", value, column.OrdinalPosition));
            }
            else
            {
                value = column.DefaultValue;
            }
            return value;
        }

        private object GetTypedValueFromString(string value, Type propertyType)
        {
            object result = null;

            try
            {
                var type = propertyType;
                if (propertyType.GenericTypeArguments.Length == 1)
                    type = propertyType.GenericTypeArguments[0];

                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.String:
                        result = value;
                        break;
                    case TypeCode.Boolean:
                        result = bool.Parse(value);
                        break;
                    case TypeCode.Char:
                        result = char.Parse(value);
                        break;
                    case TypeCode.Byte:
                        result = byte.Parse(value);
                        break;
                    case TypeCode.SByte:
                        result = sbyte.Parse(value);
                        break;
                    case TypeCode.Decimal:
                        result = decimal.Parse(value);
                        break;
                    case TypeCode.Double:
                        result = double.Parse(value);
                        break;
                    case TypeCode.Single:
                        result = float.Parse(value);
                        break;
                    case TypeCode.Int16:
                        result = short.Parse(value);
                        break;
                    case TypeCode.Int32:
                        result = int.Parse(value);
                        break;
                    case TypeCode.Int64:
                        result = long.Parse(value);
                        break;
                    case TypeCode.UInt16:
                        result = ushort.Parse(value);
                        break;
                    case TypeCode.UInt32:
                        result = uint.Parse(value);
                        break;
                    case TypeCode.UInt64:
                        result = ulong.Parse(value);
                        break;
                    case TypeCode.DateTime:
                        result = DateTime.Parse(value);
                        break;
                    default:
                        if (type == typeof(TimeSpan))
                            result = TimeSpan.Parse(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new CsvException(string.Format("GetTypedValue failed on value '{0}', with message: {1}. See inner exception for details.",
                    value,
                    ex.Message), ex);
            }
            return result;
        }

        private bool IsValueValid(string value, CsvColumn column)
        {
            return column.ValidationRegExPattern == null
                || column.ValidationRegExPattern == string.Empty
                || Regex.IsMatch(value, column.ValidationRegExPattern);
        }
    }
}

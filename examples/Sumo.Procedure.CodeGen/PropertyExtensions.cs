using Sumo.Procedure.CodeGen.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sumo.Procedure.CodeGen
{
    public static class PropertyExtensions
    {
        private static readonly string[] _templateLines = Resources.PropertyTemplate.Split(Environment.NewLine);
        private static readonly Dictionary<string, string> _templateMap = UnitFactory.CreateMap(Resources.PropertyTemplate);
        private static readonly StringBuilder _builder = new StringBuilder();
        private static readonly Type _procedureParameter = typeof(ProcedureParameter);

        public static string ToPropertyString(this ProcedureParameter parameter)
        {
            _builder.Clear();

            for (var i = 0; i < _templateLines.Length; ++i)
            {
                var line = _templateLines[i];
                foreach (var item in _templateMap)
                {
                    var property = _procedureParameter.GetProperty(item.Value);
                    if (property == null) throw new Exception($"Property not found: '{item.Value}'");
                    var value = property.GetValue(parameter).ToString();

                    var match = Regex.Match(line, item.Key);
                    if (match.Success) line = line.Replace(item.Key, value).TrimEnd();
                }
                if (!String.IsNullOrEmpty(line)) _builder.AppendLine($"        {line}");
            }

            return _builder.ToString();
        }
    }
}
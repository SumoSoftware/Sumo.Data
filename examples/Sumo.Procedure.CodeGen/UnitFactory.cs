using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sumo.Procedure.CodeGen
{
    public class UnitFactory
    {
        readonly string _template;
        public Dictionary<string, string> TemplateMap { get; }

        public UnitFactory(string template)
        {
            if (string.IsNullOrEmpty(template)) throw new ArgumentNullException(nameof(template));

            _template = template;
            TemplateMap = CreateMap(template);
        }

        public override string ToString()
        {
            var result = _template;
            foreach(var item in TemplateMap)
            {
                result = result.Replace(item.Key, item.Value);
            }
            return result;
        }
        
        public static Dictionary<string, string> CreateMap(string template)
        {
            string pattern = @"\{\w+\}";
            var templateItems = Regex.Matches(template, pattern);
            var result = new Dictionary<string, string>(templateItems.Count);
            for(var i=0;i<templateItems.Count;++i)
            {
                result.TryAdd(templateItems[i].Value, templateItems[i].Value.Replace("{", "").Replace("}", ""));
            }
            return result;
        }
    }
}

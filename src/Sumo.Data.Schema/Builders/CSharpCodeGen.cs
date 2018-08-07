using System;
using Sumo.Data.Types;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Sumo.Data.Schema.Builders
{
    public class CSharpCodeGen : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private String Tabs(int count)
        {
            var tabs = String.Empty;
            for(var idx = 0; idx < count; ++idx)
            {
                tabs += "\t";
            }

            return tabs;
        }

        private String SanitizeField(String columnName)
        {
            return  $"_{columnName.Substring(0,1).ToLower()}{columnName.Substring(1)}";
        }

        public String ToFile(IEnumerable<Table> tables, string ns, bool notifyPropertyChanged = false)
        {
            var bldr = new StringBuilder();
            bldr.AppendLine("using System;");
            bldr.AppendLine("using System.ComponentModel;");
            bldr.AppendLine("using System.Runtime.CompilerServices;");
            bldr.AppendLine();
            bldr.AppendLine($"namespace {ns}");
            bldr.AppendLine("{");
            foreach(var tbl in tables)
            {
                bldr.AppendLine(ToClass(tbl, notifyPropertyChanged: true));
            }

            bldr.AppendLine("}");


            return bldr.ToString();
        }

        public String ToClass(Table table, string modififier = "public", int tabs = 1, bool notifyPropertyChanged = false)
        {
            var bldr = new StringBuilder();

            if (notifyPropertyChanged)
            {
                bldr.AppendLine($"{Tabs(tabs)}{modififier} class {table.Name} : INotifyPropertyChanged");
                bldr.AppendLine($"{Tabs(tabs)}{{");
                bldr.AppendLine($"{Tabs(tabs + 1)}public event PropertyChangedEventHandler PropertyChanged;");
                bldr.AppendLine();
                bldr.AppendLine($"{Tabs(tabs + 1)}private void NotifyPropertyChanged([CallerMemberName] string propertyName = \"\")");
                bldr.AppendLine($"{Tabs(tabs + 1)}{{");
                bldr.AppendLine($"{Tabs(tabs + 2)}PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));");
                bldr.AppendLine($"{Tabs(tabs + 1)}}}");
                bldr.AppendLine();

            }
            else
            {
                bldr.AppendLine($"{Tabs(tabs)}{modififier} class {table.Name}");
                bldr.AppendLine($"{Tabs(tabs)}{{");
            }
            
            foreach (var col in table.Columns)
            {
                bldr.AppendLine($"{Tabs(tabs + 1)}private {col.DataType.ToType()} {SanitizeField(col.Name)};");
                bldr.AppendLine($"{Tabs(tabs + 1)}public {col.DataType.ToType()} {col.Name}");
                bldr.AppendLine($"{Tabs(tabs + 1)}{{");
                bldr.AppendLine($"{Tabs(tabs + 2)}get {{ return {SanitizeField(col.Name)}; }}");
                bldr.AppendLine($"{Tabs(tabs + 2)}set");
                bldr.AppendLine($"{Tabs(tabs + 2)}{{");
                bldr.AppendLine($"{Tabs(tabs + 3)}if({SanitizeField(col.Name)} != value)");
                bldr.AppendLine($"{Tabs(tabs + 3)}{{");
                bldr.AppendLine($"{Tabs(tabs + 4)}{SanitizeField(col.Name)} = value;");
                if (notifyPropertyChanged)
                {
                    bldr.AppendLine($"{Tabs(tabs + 4)}NotifyPropertyChanged();");
                }
                bldr.AppendLine($"{Tabs(tabs + 3)}}}");
                bldr.AppendLine($"{Tabs(tabs + 2)}}}");
                bldr.AppendLine($"{Tabs(tabs + 1)}}}");
                bldr.AppendLine();
            }
            bldr.AppendLine($"{Tabs(tabs)}}}");

            return bldr.ToString();
        }
    }
}

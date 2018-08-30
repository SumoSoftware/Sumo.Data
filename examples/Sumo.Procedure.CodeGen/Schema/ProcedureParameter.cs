using Sumo.Data;
using Sumo.Data.Schema.SqlServer;
using System.Text;

namespace Sumo.Procedure.CodeGen
{
    public class ProcedureParameter : EntityName
    {
        public ProcedureParameter() : base() { }
        public ProcedureParameter(string name) : base(name) { }
        public ProcedureParameter(string schema, string name) : base(schema, name) { }

        public string Procedure { get; set; }
        public string DataType { get; set; }
        public string Direction { get; set; }
        public int Order { get; set; }
        public int? MaxLength { get; set; }
        public string Encoding { get; set; }

        [IgnoreColumn]
        public string Attribute
        {
            get
            {

                var builder = new StringBuilder();
                if (Direction == "INOUT")
                {
                    if (MaxLength.HasValue)
                        builder.AppendLine($"[InputOutputParameter({MaxLength.Value})]");
                    else
                        builder.AppendLine("[InputOutputParameter]");
                }
                if(MaxLength.HasValue) builder.AppendLine($"[InputParameter({MaxLength.Value})]");
                return builder.ToString();
            }
        }

        [IgnoreColumn]
        public string Type
        {
            get
            {
                var sqlType = DataType.ToSqlDbType();
                return sqlType.ToType().Name;
            }
        }
    }
}

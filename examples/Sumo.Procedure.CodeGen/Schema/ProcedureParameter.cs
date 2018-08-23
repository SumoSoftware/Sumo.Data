using Sumo.Data;
using Sumo.Data.Schema.SqlServer;

namespace Sumo.Procedure.CodeGen
{
    public class ProcedureParameter : EntityName
    {
        public ProcedureParameter() : base() { }
        public ProcedureParameter(string name) : base(name) { }
        public ProcedureParameter(string schema, string name) : base(schema, name) { }

        public string Procedure { get; internal set; }
        public string DataType { get; internal set; }
        public string Direction { get; internal set; }
        public int Order { get; internal set; }
        public int? MaxLength { get; internal set; }
        public string Encoding { get; internal set; }

        [IgnoreColumn]
        public string Attribute
        {
            get
            {
                return Direction != "INOUT" ? string.Empty : "[InputOutputParameter]";
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

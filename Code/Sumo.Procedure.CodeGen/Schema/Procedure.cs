using Sumo.Data.Attributes;
using Sumo.Data.Names;

namespace Sumo.Procedure.CodeGen
{
    public class Procedure : EntityName
    {
        protected Procedure() { }

        public Procedure(string name) : base(name) { }

        public Procedure(string schema, string name) : base(schema, name) { }

        [IgnoreProperty]
        public ProcedureParameter[] ProcedureParameters { get; internal set; }
    }
}

using Sumo.Data;
using System.Collections.Generic;

namespace Sumo.Procedure.CodeGen
{
    public class Schema
    {
        public IItemName[] Schemas { get; set; }
        public IEntityName[] Tables { get; set; }
        public Dictionary<string, Procedure> Procedures { get; set; } = new Dictionary<string, Procedure>();
    }
}

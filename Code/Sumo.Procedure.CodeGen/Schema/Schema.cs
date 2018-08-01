using Sumo.Data.Names;
using System.Collections.Generic;

namespace Sumo.Procedure.CodeGen
{
    public class Schema
    {
        public IItemName[] Schemas { get; internal set; }
        public IEntityName[] Tables { get; internal set; }
        public Dictionary<string, Procedure> Procedures { get; internal set; } = new Dictionary<string, Procedure>();
    }
}

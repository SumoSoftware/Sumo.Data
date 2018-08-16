using System;

namespace Sumo.Data.Schema
{
    [Serializable]
    public class ForeignKeyDefinition 
    {
        public string Schema { get; set; } = null;
        public string Table { get; set; }
        public string Column { get; set; }
    }
}

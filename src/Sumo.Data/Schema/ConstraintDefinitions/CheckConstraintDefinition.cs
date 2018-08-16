using System;

namespace Sumo.Data.Schema
{
    [Serializable]
    public class CheckConstraintDefinition 
    {
        public string Expression { get; set; }
    }
}

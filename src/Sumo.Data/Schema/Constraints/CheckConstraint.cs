using System;

namespace Sumo.Data.Schema
{
    [Serializable]
    public class CheckConstraint 
    {
        public string Expression { get; set; }
    }
}

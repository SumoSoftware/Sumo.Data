using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Sumo.Data.Schema
{
    [Serializable]
    public class NotNullConstraint 
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ConflictClauses? ConflictClause { get; set; } = null;
    }
}

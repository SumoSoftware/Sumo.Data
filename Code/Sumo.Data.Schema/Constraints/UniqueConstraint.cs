using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sumo.Data.Schema
{
    public class UniqueConstraint 
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ConflictClauses? ConflictClause { get; set; } = null;
    }
}

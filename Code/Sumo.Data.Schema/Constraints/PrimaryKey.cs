using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sumo.Data.Expressions;

namespace Sumo.Data.Schema
{
    public class PrimaryKey
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Directions Direction { get; set; } = Directions.Ascending;

        [JsonConverter(typeof(StringEnumConverter))]
        public ConflictClauses? ConflictClause { get; set; } = null;

        public bool IsAutoIncrement { get; set; } = true;
    }
}

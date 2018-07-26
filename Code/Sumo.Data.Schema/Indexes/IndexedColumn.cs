using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sumo.Data.Expressions;

namespace Sumo.Data.Schema
{
    public class IndexedColumn : Entity
    {
        public IndexedColumn() : base() { }
        public IndexedColumn(string name, Directions direction = Directions.Ascending) : base(name)
        {
            Direction = direction;
        }
        public IndexedColumn(Column column, Directions direction = Directions.Ascending) : base(column.Name)
        {
            Direction = direction;
        }

        public string CollationName { get; set; } = null;

        [JsonConverter(typeof(StringEnumConverter))]
        public Directions Direction { get; set; } = Directions.Ascending;

        public int OrdinalPosition { get; set; } = 0;

        public override string ToString()
        {
            return $"{base.ToString()} {Direction}";
        }
    }
}

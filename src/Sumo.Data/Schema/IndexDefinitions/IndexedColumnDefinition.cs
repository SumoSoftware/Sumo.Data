using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sumo.Data.Expressions;
using System;

namespace Sumo.Data.Schema
{
    [Serializable]
    public class IndexedColumnDefinition : EntityDefinition
    {
        public IndexedColumnDefinition() : base() { }
        public IndexedColumnDefinition(string name, Directions direction = Directions.Ascending) : base(name)
        {
            Direction = direction;
        }
        public IndexedColumnDefinition(ColumnDefinition column, Directions direction = Directions.Ascending) : base(column.Name)
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

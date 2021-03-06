﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sumo.Data.Expressions;
using System;

namespace Sumo.Data.Schema
{
    [Serializable]
    public class PrimaryKeyDefinition
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Directions Direction { get; set; } = Directions.Ascending;

        [JsonConverter(typeof(StringEnumConverter))]
        public ConflictClauses? ConflictClause { get; set; } = null;

        public bool IsAutoIncrement { get; set; } = true;

        public long Seed { get; set; } = 0;
        public long Step { get; set; } = 0;
    }
}

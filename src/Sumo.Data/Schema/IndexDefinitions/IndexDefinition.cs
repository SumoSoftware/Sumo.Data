using Sumo.Data.Expressions;
using System;
using System.Collections.Generic;

namespace Sumo.Data.Schema
{
    [Serializable]
    public class IndexDefinition : EntityDefinition
    {
        public IndexDefinition() : base() { }
        public IndexDefinition(string name) : base(name) { }

        public bool IsUnique { get; set; } = false;
        public List<IndexedColumnDefinition> IndexedColumns { get; set; } = null;
        public List<EntityDefinition> CoveringColumns { get; set; } = null;
        public string FilterExpression { get; set; } = null;

        public IndexedColumnDefinition AddColumn(ColumnDefinition column, Directions direction = Directions.Ascending)
        {
            return AddColumn(new IndexedColumnDefinition(column, direction));
        }

        public IndexedColumnDefinition AddColumn(string name, Directions direction = Directions.Ascending)
        {
            return AddColumn(new IndexedColumnDefinition(name, direction));
        }

        public IndexedColumnDefinition AddColumn(IndexedColumnDefinition column)
        {
            if (column == null) throw new ArgumentNullException(nameof(column));
            if (IndexedColumns == null) IndexedColumns = new List<IndexedColumnDefinition>();

            column.OrdinalPosition = IndexedColumns.Count + 1;
            IndexedColumns.Add(column);
            return column;
        }

        public void AddCoveringColumn(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (CoveringColumns == null) CoveringColumns = new List<EntityDefinition>();

            CoveringColumns.Add(new EntityDefinition(name));
        }

        public override string ToString()
        {
            
            return $"{base.ToString()}{(IsUnique ? " UNIQUE" : string.Empty)}{(IndexedColumns != null ? $" ON {string.Join(", ", IndexedColumns)}" : string.Empty)}";
        }
    }
}

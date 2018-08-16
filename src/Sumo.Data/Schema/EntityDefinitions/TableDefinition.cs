using System;
using System.Collections.Generic;
using System.Data;

namespace Sumo.Data.Schema
{
    /// <summary>
    /// A table is the basic building block of a schema. It represents an entity or class within a larger model. 
    /// A table is an agregate of columns, keys, indexes, and constraints
    /// </summary>
    [Serializable]
    public class TableDefinition : EntityDefinition
    {
        public TableDefinition() : base() { }
        public TableDefinition(string name) : base(name) { }

        public List<ColumnDefinition> Columns { get; set; } = null;
        public List<IndexDefinition> Indexes { get; set; } = null;
        //todo: implement triggers
        //public List<Trigger> Triggers { get; set; } = null;

        public bool HasCheckConstraint
        {
            get => CheckConstraint != null;
            set
            {
                if (value && CheckConstraint == null)
                {
                    CheckConstraint = new CheckConstraintDefinition();
                }
                else if (!value && CheckConstraint != null)
                {
                    CheckConstraint = null;
                }
            }
        }
        public CheckConstraintDefinition CheckConstraint { get; set; } = null;

        public ColumnDefinition AddColumn(string name, DbType dataType)
        {
            return AddColumn(new ColumnDefinition(name, dataType));
        }

        public ColumnDefinition AddColumn(ColumnDefinition column)
        {
            if (column == null) throw new ArgumentNullException(nameof(column));
            if (Columns == null) Columns = new List<ColumnDefinition>();

            column.OrdinalPosition = Columns.Count + 1;
            Columns.Add(column);
            return column;
        }

        public IndexDefinition AddIndex(string name)
        {
            return AddIndex(new IndexDefinition(name));
        }

        public IndexDefinition AddIndex(IndexDefinition index)
        {
            if (index == null) throw new ArgumentNullException(nameof(index));
            if (Indexes == null) Indexes = new List<IndexDefinition>();

            Indexes.Add(index);
            return index;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Columns: {(Columns != null ? Columns.Count : 0)}";
        }
    }
}

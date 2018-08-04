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
    public class Table : Entity
    {
        public Table() : base() { }
        public Table(string name) : base(name) { }

        public List<Column> Columns { get; set; } = null;
        public List<Index> Indexes { get; set; } = null;
        //todo: implement triggers
        //public List<Trigger> Triggers { get; set; } = null;

        public bool HasCheckConstraint
        {
            get => CheckConstraint != null;
            set
            {
                if (value && CheckConstraint == null)
                {
                    CheckConstraint = new CheckConstraint();
                }
                else if (!value && CheckConstraint != null)
                {
                    CheckConstraint = null;
                }
            }
        }
        public CheckConstraint CheckConstraint { get; set; } = null;

        public Column AddColumn(string name, DbType dataType)
        {
            return AddColumn(new Column(name, dataType));
        }

        public Column AddColumn(Column column)
        {
            if (column == null) throw new ArgumentNullException(nameof(column));
            if (Columns == null) Columns = new List<Column>();

            column.OrdinalPosition = Columns.Count + 1;
            Columns.Add(column);
            return column;
        }

        public Index AddIndex(string name)
        {
            return AddIndex(new Index(name));
        }

        public Index AddIndex(Index index)
        {
            if (index == null) throw new ArgumentNullException(nameof(index));
            if (Indexes == null) Indexes = new List<Index>();

            Indexes.Add(index);
            return index;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Columns: {(Columns != null ? Columns.Count : 0)}";
        }
    }
}

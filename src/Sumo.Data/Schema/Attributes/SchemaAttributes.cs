using Sumo.Data.Expressions;
using System;

// these attributes allow an sql based ORM to create tables
namespace Sumo.Data.Schema
{
    /// <summary>
    /// Indicates this item is part of the primary key.
    /// Apply to multiple properties for multi-column keys.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : DataAttribute
    {
        public PrimaryKeyAttribute(Directions direction = Directions.Ascending, bool autoIncrement = true) : base()
        {
            Direction = direction;
            AutoIncrement = autoIncrement;
        }

        public Directions Direction { get; }
        public bool AutoIncrement { get; }
    }

    /// <summary>
    /// Same as not null attribute on a db column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : DataAttribute
    {
    }

    /// <summary>
    /// The value can't be repeated in the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : DataAttribute
    {
    }

    /// <summary>
    /// Maximum size for strings. size parameter must be > 0.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxSizeAttribute : DataAttribute
    {
        public MaxSizeAttribute(int size) : base()
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            Size = size;
        }

        public int Size { get; }
    }

    /// <summary>
    /// Default value for column in database if field is null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultValueAttribute : DataAttribute
    {
        public DefaultValueAttribute(string defaultValue) : base()
        {
            if (string.IsNullOrEmpty(defaultValue)) throw new ArgumentNullException(nameof(defaultValue));
            Value = defaultValue;
        }

        public string Value { get; }
    }

    /// <summary>
    /// entity comments
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class CommentAttribute : DataAttribute
    {
        public CommentAttribute(string comment) : base()
        {
            if (string.IsNullOrEmpty(comment)) throw new ArgumentNullException(nameof(comment));
            Comment = comment;
        }

        public string Comment { get; }
    }

    /// <summary>
    /// entity comments
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : DataAttribute
    {
        public string Schema { get; }
        public string ReferenceTable { get; }
        public string ReferenceColumn { get; }

        public ForeignKeyAttribute(Type referenceTable, string referenceColumn) : this(referenceTable.Name, referenceColumn) { }

        public ForeignKeyAttribute(string schema, Type referenceTable, string referenceColumn) : this(schema, referenceTable.Name, referenceColumn) { }

        public ForeignKeyAttribute(string referenceTable, string referenceColumn) : this(null, referenceTable, referenceColumn) { }

        public ForeignKeyAttribute(string schema, string referenceTable, string referenceColumn) : base()
        {
            if (string.IsNullOrEmpty(referenceTable)) throw new ArgumentNullException(nameof(referenceTable));
            if (string.IsNullOrEmpty(referenceColumn)) throw new ArgumentNullException(nameof(referenceColumn));

            Schema = schema;
            ReferenceTable = referenceTable;
            ReferenceColumn = referenceColumn;
        }
    }
}

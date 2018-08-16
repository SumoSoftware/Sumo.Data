using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Data;

namespace Sumo.Data.Schema
{
    /// <summary>
    /// A column is the basic building block of a table. It represents an element or property of a modeled entity or class.
    /// </summary>
    [Serializable]
    public class ColumnDefinition : EntityDefinition
    {
        public ColumnDefinition() : base() { }
        public ColumnDefinition(string name, DbType dataType) : base(name)
        {
            DataType = dataType;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public DbType DataType { get; set; } = DbType.Int32;

        public int OrdinalPosition { get; set; } = 0;

        public string Default { get; set; } = null;

        public bool IsPrimaryKey
        {
            get => PrimaryKey != null;
            set
            {
                if (value && PrimaryKey == null)
                {
                    PrimaryKey = new PrimaryKeyDefinition();
                    IsNullable = false;
                    IsUnique = false;
                    HasCheckConstraint = false;
                    HasForeignKey = false;
                }
                else if (!value && PrimaryKey != null)
                {
                    PrimaryKey = null;
                }
            }
        }
        public PrimaryKeyDefinition PrimaryKey { get; set; } = null;

        public bool IsNullable
        {
            get => NotNullConstraint == null;
            set
            {
                if (!value && NotNullConstraint == null)
                {
                    NotNullConstraint = new NotNullConstraintDefinition();
                }
                else if (value && !IsPrimaryKey && NotNullConstraint != null)
                {
                    NotNullConstraint = null;
                }
            }
        }
        public NotNullConstraintDefinition NotNullConstraint { get; set; } = null;

        public bool HasCheckConstraint
        {
            get => CheckConstraint != null;
            set
            {
                if (value && !IsPrimaryKey && CheckConstraint == null)
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

        public bool IsUnique
        {
            get => UniqueConstraint != null;
            set
            {
                if (value && !IsPrimaryKey && UniqueConstraint == null)
                {
                    UniqueConstraint = new UniqueConstraintDefinition();
                    IsNullable = false;
                }
                else if (!value && UniqueConstraint != null)
                {
                    UniqueConstraint = null;
                }
            }
        }
        public UniqueConstraintDefinition UniqueConstraint { get; set; } = null;

        public bool HasForeignKey
        {
            get => ForeignKey != null;
            set
            {
                if (value && !IsPrimaryKey && ForeignKey == null)
                {
                    ForeignKey = new ForeignKeyDefinition();
                }
                else if (!value && ForeignKey != null)
                {
                    ForeignKey = null;
                }
            }
        }
        public ForeignKeyDefinition ForeignKey { get; set; } = null;

        public string CollationName { get; set; } = null;

        /// <summary>
        /// null for MAX
        /// </summary>
        public int? MaxLength { get; set; } = null;

        /// <summary>
        /// null for default
        /// </summary>
        public int? Precision { get; set; } = null;

        public override string ToString()
        {
            return $"{base.ToString()} {DataType}{(IsPrimaryKey ? " PK" : (!IsNullable ? " NOT NULL" : string.Empty))}{(string.IsNullOrEmpty(Default) ? string.Empty : $" {Default}")}";
        }
    }
}

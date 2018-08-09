using System;
using System.Collections.Generic;
using System.Text;

namespace Sumo.Data.SqlExpressions
{
    //todo: implement IEquatable on table classes
    public class FromTable : SelectTable, IFromTable
    {
        public FromTable(IEntityName tableName, IItemName[] selectColumns) : base(tableName, selectColumns)
        {
            if (selectColumns == null) throw new ArgumentNullException(nameof(selectColumns));
            if (Columns.Length == 0) throw new ArgumentException($"{nameof(selectColumns)} array is empty.");
        }

        public List<IJoinTable> JoinTables { get; private set; } = null;

        public void Join(IJoinTable table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (JoinTables == null) JoinTables = new List<IJoinTable>();

            JoinTables.Add(table);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"select {GetSelectClause()}");
            builder.AppendLine($"from {TableName}");
            if (JoinTables != null)
            {
                foreach (var joinTable in JoinTables)
                {
                    builder.AppendLine(joinTable.ToString());
                }
            }
            //todo: write where clause to string

            return builder.ToString();
        }

        internal void AddColumnsToSelectClause(ISelectTable table, IItemName[] columns, StringBuilder builder, bool firstPass = false)
        {
            if (columns == null || columns.Length == 0) return;

            for (var i = 0; i < columns.Length; ++i)
            {
                var column = columns[i];
                if (!firstPass || i > 0) builder.Append(", ");
                builder.Append($"{table.TableName}.{column}");
            }
        }

        internal string GetSelectClause()
        {
            var builder = new StringBuilder();

            AddColumnsToSelectClause(this, Columns, builder, true);

            if (JoinTables != null)
            {
                foreach (var joinTable in JoinTables)
                {
                    AddColumnsToSelectClause(joinTable, joinTable.Columns, builder);
                }
            }

            return builder.ToString();
        }
    }
}

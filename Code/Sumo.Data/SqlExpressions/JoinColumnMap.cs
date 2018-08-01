using Sumo.Data.Expressions;
using Sumo.Data.Names;
using System;
using System.Collections.Generic;

namespace Sumo.Data.SqlExpressions
{
    public class JoinColumn : IEquatable<JoinColumn>
    {
        public JoinColumn(IEntityName tableName, IItemName columnName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            ColumnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
        }
        public IEntityName TableName { get; }
        public IItemName ColumnName { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JoinColumn);
        }

        public bool Equals(JoinColumn other)
        {
            return other != null &&
                   EqualityComparer<IEntityName>.Default.Equals(TableName, other.TableName) &&
                   EqualityComparer<IItemName>.Default.Equals(ColumnName, other.ColumnName);
        }

        public override int GetHashCode()
        {
            var hashCode = -1507518432;
            hashCode = hashCode * -1521134295 + EqualityComparer<IEntityName>.Default.GetHashCode(TableName);
            hashCode = hashCode * -1521134295 + EqualityComparer<IItemName>.Default.GetHashCode(ColumnName);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{TableName}.{ColumnName}";
        }

        public static bool operator ==(JoinColumn column1, JoinColumn column2)
        {
            return EqualityComparer<JoinColumn>.Default.Equals(column1, column2);
        }

        public static bool operator !=(JoinColumn column1, JoinColumn column2)
        {
            return !(column1 == column2);
        }
    }

    public class JoinColumnMap : IEquatable<JoinColumnMap>
    {
        public JoinColumnMap(JoinColumn leftColumn, JoinColumn rightColumn,
            RelationalOperators relationalOperator = RelationalOperators.Equal,
            LogicalOperators logicalOperator = LogicalOperators.And)
        {
            LeftColumn = leftColumn ?? throw new ArgumentNullException(nameof(leftColumn));
            RightColumn = rightColumn ?? throw new ArgumentNullException(nameof(rightColumn));
            RelationalOperator = relationalOperator;
            LogicalOperator = logicalOperator;
        }

        public JoinColumn LeftColumn { get; }
        public JoinColumn RightColumn { get; }
        public RelationalOperators RelationalOperator { get; }
        public LogicalOperators LogicalOperator { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JoinColumnMap);
        }

        public bool Equals(JoinColumnMap other)
        {
            return other != null &&
                   EqualityComparer<JoinColumn>.Default.Equals(LeftColumn, other.LeftColumn) &&
                   EqualityComparer<JoinColumn>.Default.Equals(RightColumn, other.RightColumn) &&
                   RelationalOperator == other.RelationalOperator &&
                   LogicalOperator == other.LogicalOperator;
        }

        public override int GetHashCode()
        {
            var hashCode = -572934366;
            hashCode = hashCode * -1521134295 + EqualityComparer<JoinColumn>.Default.GetHashCode(LeftColumn);
            hashCode = hashCode * -1521134295 + EqualityComparer<JoinColumn>.Default.GetHashCode(RightColumn);
            hashCode = hashCode * -1521134295 + RelationalOperator.GetHashCode();
            hashCode = hashCode * -1521134295 + LogicalOperator.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{LeftColumn}{RelationalOperator.ToSqlString()}{RightColumn}";
        }

        public static bool operator ==(JoinColumnMap map1, JoinColumnMap map2)
        {
            return EqualityComparer<JoinColumnMap>.Default.Equals(map1, map2);
        }

        public static bool operator !=(JoinColumnMap map1, JoinColumnMap map2)
        {
            return !(map1 == map2);
        }
    }
}

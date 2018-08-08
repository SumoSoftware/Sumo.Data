using Sumo.Data.Names;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sumo.Data.SqlExpressions
{
    //todo: implement IEquatable on table classes
    public class JoinTable : SelectTable, IJoinTable
    {
        public JoinTable(IEntityName tableName) : base(tableName) { }

        public JoinTable(IEntityName tableName, IItemName[] selectColumns) : base(tableName, selectColumns) { }

        private List<JoinColumnMap> _joinColumnMap = new List<JoinColumnMap>();

        public void AddJoinColumn(JoinColumnMap map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            _joinColumnMap.Add(map);
        }

        public void AddJoinColumn(JoinColumn leftColumn, JoinColumn rightColumn,
            RelationalOperators relationalOperator = RelationalOperators.Equal,
            LogicalOperators logicalOperator = LogicalOperators.And)
        {
            AddJoinColumn(new JoinColumnMap(leftColumn, rightColumn, relationalOperator, logicalOperator));
        }

        public void AddJoinColumn(
            IEntityName leftTable,
            IItemName leftColumnName,
            IItemName rightColumnName,
            RelationalOperators relationalOperator = RelationalOperators.Equal,
            LogicalOperators logicalOperator = LogicalOperators.And)
        {
            var leftColumn = new JoinColumn(leftTable, leftColumnName);
            AddJoinColumn(leftColumn, GetJoinColumn(rightColumnName), relationalOperator, logicalOperator);
        }

        public JoinColumn GetJoinColumn(IItemName rightColumnName)
        {
            return new JoinColumn(TableName, rightColumnName);
        }

        //todo: it would be more effective to use IExpression instead of a list of columns
        public override string ToString()
        {
            if (_joinColumnMap.Count == 0) return string.Empty;

            var builder = new StringBuilder();

            builder.Append($"join {TableName} on ");
            var firstPass = true;
            foreach (var map in _joinColumnMap)
            {
                if (!firstPass) builder.Append($" {map.LogicalOperator.ToSqlString()} ");
                builder.Append(map);
                firstPass = false;
            }

            return builder.ToString();
        }
    }
}

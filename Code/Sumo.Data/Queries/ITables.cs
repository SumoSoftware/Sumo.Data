using Sumo.Data.Expressions;
using Sumo.Data.Names;
using System.Collections.Generic;

namespace Sumo.Data.Queries
{
    public interface ISelectTable
    {
        IEntityName TableName { get; }
        IItemName[] Columns { get; }
    }

    public interface IFromTable : ISelectTable
    {
        List<IJoinTable> JoinTables { get; }
        void Join(IJoinTable table);
    }

    public interface IJoinTable : ISelectTable
    {
        void AddJoinColumn(JoinColumnMap map);

        void AddJoinColumn(
            JoinColumn leftColumn, JoinColumn rightColumn, 
            RelationalOperators relationalOperator = RelationalOperators.Equal, 
            LogicalOperators logicalOperator = LogicalOperators.And);

        void AddJoinColumn(
            IEntityName leftTable, IItemName leftColumnNane,
            IItemName rightColumnName,
            RelationalOperators relationalOperator = RelationalOperators.Equal, 
            LogicalOperators logicalOperator = LogicalOperators.And);

        JoinColumn GetJoinColumn(IItemName rightColumnName);
    }
}

using Sumo.Data.Names;
using System;

namespace Sumo.Data.SqlExpressions
{
    //todo: implement IEquatable on table classes
    public abstract class SelectTable : ISelectTable
    {
        public SelectTable(IEntityName tableName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Columns = null;
        }

        public SelectTable(IEntityName tableName, IItemName[] columns)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Columns = columns;
            if (Columns != null)
            {
                for (var i = 0; i < Columns.Length; ++i)
                {
                    if (Columns[i] == null)
                    {
                        throw new ArgumentException($"{nameof(columns)}[{i}] is null.");
                    }
                }
            }
        }

        public IEntityName TableName { get; }
        public IItemName[] Columns { get; }
    }
}

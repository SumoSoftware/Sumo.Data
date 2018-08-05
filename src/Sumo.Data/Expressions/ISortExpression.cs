using Sumo.Data.Names;
using System;

namespace Sumo.Data.Expressions
{
    public interface ISortExpression: IEquatable<ISortExpression>
    {
        void Join(IItemName columnName, Directions sortDirection = Directions.Ascending);
        void Join(ISortExpression sortExpression);

        ISortExpression NextExpression { get; }
    }
}

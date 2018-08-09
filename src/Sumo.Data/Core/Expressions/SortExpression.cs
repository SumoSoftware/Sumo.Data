using System;
using System.Collections.Generic;
using System.Text;

namespace Sumo.Data.Expressions
{
    //todo: add tests for sort expression

    public class SortExpression : ISortExpression, IEquatable<SortExpression>
    {
        private readonly IItemName _columnName;
        private readonly Directions _sortDirection;

        public SortExpression(IItemName columnName, Directions sortDirection = Directions.Ascending)
        {
            _columnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
            _sortDirection = sortDirection;
        }

        public void Join(IItemName columnName, Directions sortDirection = Directions.Ascending)
        {
            NextExpression = new SortExpression(columnName, sortDirection);
        }

        public void Join(ISortExpression sortExpression)
        {
            NextExpression = sortExpression;
        }

        public ISortExpression NextExpression { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder($"{_columnName} {_sortDirection.ToSqlString()}");

            var sortExpression = NextExpression;
            while (sortExpression != null)
            {
                builder.Append(", ");
                builder.Append(sortExpression);
                sortExpression = sortExpression.NextExpression;
            }

            return builder.ToString();
        }


        #region IEquatable

        public override bool Equals(object obj)
        {
            return Equals(obj as SortExpression);
        }

        public bool Equals(SortExpression other)
        {
            return other != null &&
                   _columnName == other._columnName &&
                   _sortDirection == other._sortDirection &&
                   NextExpression.Equals(other.NextExpression);
        }

        public bool Equals(ISortExpression other)
        {
            if (other is SortExpression)
            {
                return Equals((SortExpression)other);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = -1229568524;
            hashCode = hashCode * -1521134295 + EqualityComparer<IItemName>.Default.GetHashCode(_columnName);
            hashCode = hashCode * -1521134295 + _sortDirection.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ISortExpression>.Default.GetHashCode(NextExpression);
            return hashCode;
        }

        public static bool operator ==(SortExpression sortExpression1, SortExpression sortExpression2)
        {
            return EqualityComparer<SortExpression>.Default.Equals(sortExpression1, sortExpression2);
        }

        public static bool operator !=(SortExpression sortExpression1, SortExpression sortExpression2)
        {
            return !(sortExpression1 == sortExpression2);
        }
        #endregion
    }
}
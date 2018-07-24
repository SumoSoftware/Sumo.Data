namespace Sumo.Data.Expressions
{
    public enum SortDirections
    {
        Ascending = 1,
        Descending = 2
    }

    public static class SortDirectionsExtensions
    {
        public static string ToSqlString(this SortDirections sortDirection)
        {
            switch (sortDirection)
            {
                case SortDirections.Descending:
                    return "DESC";
                case SortDirections.Ascending:
                default:
                    return "ASC";
            }
        }
    }
}

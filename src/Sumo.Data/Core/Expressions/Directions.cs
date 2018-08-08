namespace Sumo.Data
{
    public enum Directions
    {
        Ascending = 1,
        Descending = 2
    }

    public static class SortDirectionsExtensions
    {
        public static string ToSqlString(this Directions sortDirection)
        {
            switch (sortDirection)
            {
                case Directions.Descending:
                    return "DESC";
                case Directions.Ascending:
                default:
                    return "ASC";
            }
        }
    }
}

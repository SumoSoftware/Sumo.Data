namespace Sumo.Data.Expressions
{
    public enum LogicalOperators
    {
        And,
        Or
    }

    public enum RelationalOperators
    {
        Equal,
        NotEqual,

        GreaterThan,
        GreaterThanOrEqual,

        LessThan,
        LessThanOrEqual,

        Like,
        NotLike,

        //IsNull,
        //IsNotNull
    }

    public static class OperatorExtensions
    {
        public static string ToSqlString(this LogicalOperators relationalOperator)
        {
            return relationalOperator.ToString().ToLowerInvariant();
        }

        public static string ToSqlString(this RelationalOperators relationalOperator)
        {
            var result = string.Empty;
            switch (relationalOperator)
            {
                case RelationalOperators.Equal:
                    result = "=";
                    break;
                case RelationalOperators.NotEqual:
                    result = "!=";
                    break;
                case RelationalOperators.GreaterThan:
                    result = ">";
                    break;
                case RelationalOperators.GreaterThanOrEqual:
                    result = ">=";
                    break;
                case RelationalOperators.LessThan:
                    result = "<";
                    break;
                case RelationalOperators.LessThanOrEqual:
                    result = "<=";
                    break;
                case RelationalOperators.Like:
                    result = "like";
                    break;
                case RelationalOperators.NotLike:
                    result = "not like";
                    break;
                    //case RelationalOperators.IsNull:
                    //case RelationalOperators.IsNotNull:
                    //    throw new NotSupportedException($"{nameof(relationalOperator)} value of {relationalOperator.ToString()} can't be converted to String.");
            }
            return result;
        }
    }
}

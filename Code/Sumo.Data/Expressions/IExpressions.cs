using Sumo.Data.Names;
using Sumo.Data.SqlExpressions;
using System.Collections.Generic;

namespace Sumo.Data.Expressions
{
    public interface IExpression
    {
        Dictionary<string, object> GetParameters();
    }

    public interface IBinaryExpression : IExpression
    {
        LogicalOperators Operator { get; }
        IExpression LeftOperand { get; }
        IExpression RightOperand { get; }
    }

    public interface IColumnExpression : IExpression
    {
        IItemName ColumnName { get; }
    }

    public interface IValueExpression : IExpression
    {
        object Value { get; }
    }

    public interface IParameterExpression : IColumnExpression, IValueExpression
    {
        IParameterName ParameterName { get; }
    }

    public interface IIsNullExpression : IColumnExpression { }
    public interface IIsNotNullExpression : IColumnExpression { }

    public interface IRelationalExpression : IParameterExpression
    {
        RelationalOperators RelationalOperator { get; }
    }

    public enum ContainsExpressionTypes
    {
        SubQuery,
        Collection
    }

    public interface ISubQueryExpression : IExpression
    {
        SqlExpression SubQuery { get; }
    }

    public interface IContainedExpression : IColumnExpression, ISubQueryExpression // sql 'in'
    {
        ContainsExpressionTypes ExpressionType { get; }
        string[] CollectionValues { get; }
    }
    public interface INotContainedExpression : IContainedExpression { } // sql 'not in'

    public interface IExistsExpression : ISubQueryExpression { }
    public interface INotExistsExpression : IExistsExpression { }

    /// <summary>
    /// https://docs.microsoft.com/en-us/sql/t-sql/language-elements/some-any-transact-sql?view=sql-server-2017
    /// http://bradsruminations.blogspot.com/2009/08/all-any-and-some-three-stooges.html
    /// </summary>
    public interface ISomeExpression : ISubQueryExpression
    {
        RelationalOperators Operator { get; }
    }

    public interface IAllExpression : ISubQueryExpression
    {
        RelationalOperators Operator { get; }
    }

    public interface IBetweenExpresion<T> : IColumnExpression
    {
        T LeftOperand { get; }
        T RightOperand { get; }
    }
    public interface INotBetweenExpression<T> : IBetweenExpresion<T> { }

    public enum FunctionalExpressionLeftOperandTypes
    {
        Parameter,
        Column
    }

    public interface IFunctionalExpression : IExpression
    {
        FunctionalExpressionLeftOperandTypes LeftOperandType { get; }
        RelationalOperators Operator { get; }
        // function is right operand
        string FunctionName { get; }
        object[] FunctionParameterValues { get; }
        string LeftOperandName { get; }
    }
}

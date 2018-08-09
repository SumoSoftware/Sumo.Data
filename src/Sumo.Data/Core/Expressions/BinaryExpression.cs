using System;
using System.Collections.Generic;

namespace Sumo.Data.Expressions
{
    public sealed class BinaryExpression : IBinaryExpression
    {
        public BinaryExpression(IExpression leftOperand, IExpression rightOperand, LogicalOperators logicalOperator = LogicalOperators.And)
        {
            LeftOperand = leftOperand ?? throw new ArgumentNullException(nameof(leftOperand));
            RightOperand = rightOperand ?? throw new ArgumentNullException(nameof(rightOperand));
            Operator = logicalOperator;
        }

        public LogicalOperators Operator { get; }
        public IExpression LeftOperand { get; }
        public IExpression RightOperand { get; }

        public Dictionary<string, object> GetParameters()
        {
            throw new NotImplementedException();
        }
    }
}

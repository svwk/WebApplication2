using System.Linq.Expressions;

namespace WebApplication2
{
    public class ParameterReplaceVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression from, to;

        public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
        {
            this.from = from;
            this.to = to;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == from ? to : base.VisitParameter(node);
        }
    }
}
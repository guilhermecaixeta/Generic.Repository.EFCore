using System.Linq.Expressions;

namespace Generic.Repository.Extension.Filter.Facade
{
    public class ExpressionUpdatingFacade : ExpressionVisitor
    {
        private Expression Expression;

        private ConstantExpression constantExpression;

        public ExpressionUpdatingFacade(Expression expression) =>
            Expression = expression;

        public Expression SetNewConstantExpression(object value)
        {
            this.Expression.Reduce();
            constantExpression = Expression.Constant(value);

            return Visit(Expression);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            return base.VisitConstant(constantExpression);
        }

    }
}

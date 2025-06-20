using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Utils
{
    public class ExpressionBuilder<T>
    {
        private Expression<Func<T, bool>> _expression;

        public ExpressionBuilder()
        {
            // 初始化一个始终为true的表达式
            _expression = x => true;
        }

        public ExpressionBuilder<T> And(Expression<Func<T, bool>> expression)
        {
            if (_expression == null)
            {
                _expression = expression;
            }
            else
            {
                var parameter = Expression.Parameter(typeof(T));
                var leftVisitor = new ReplaceParameterVisitor(_expression.Parameters[0], parameter);
                var left = leftVisitor.Visit(_expression.Body);
                var rightVisitor = new ReplaceParameterVisitor(expression.Parameters[0], parameter);
                var right = rightVisitor.Visit(expression.Body);

                _expression = Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(left, right), parameter);
            }
            return this;
        }

        public ExpressionBuilder<T> Or(Expression<Func<T, bool>> expression)
        {
            if (_expression == null)
            {
                _expression = expression;
            }
            else
            {
                var parameter = Expression.Parameter(typeof(T));
                var leftVisitor = new ReplaceParameterVisitor(_expression.Parameters[0], parameter);
                var left = leftVisitor.Visit(_expression.Body);
                var rightVisitor = new ReplaceParameterVisitor(expression.Parameters[0], parameter);
                var right = rightVisitor.Visit(expression.Body);

                _expression = Expression.Lambda<Func<T, bool>>(
                    Expression.OrElse(left, right), parameter);
            }
            return this;
        }

        public Expression<Func<T, bool>> Build()
        {
            return _expression;
        }

        // 辅助类，用于替换表达式中的参数
        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : base.VisitParameter(node);
            }
        }
    }
}

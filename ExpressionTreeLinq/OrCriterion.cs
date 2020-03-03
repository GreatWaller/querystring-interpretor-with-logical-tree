using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionTreeLinq
{
    class OrCriterion<T> : ICriterion<T>
    {
        private readonly ICriterion<T> _left;
        private readonly ICriterion<T> _right;

        public OrCriterion(ICriterion<T> left, ICriterion<T> right)
        {
            _left = left;
            _right = right;
        }

        public Expression HandleExpression(ParameterExpression pe)
        {
            return Expression.OrElse(_left.HandleExpression(pe),
                _right.HandleExpression(pe));
        }

        public IQueryable<T> HandleQueryable(IQueryable<T> query)
        {
            var left = _left.HandleQueryable(query);
            var right = _right.HandleQueryable(query);
            return left.Union(right);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionTreeLinq
{
    public class AndCriterion<T> : ICriterion<T>
    {
        private readonly ICriterion<T> _left;
        private readonly ICriterion<T> _right;

        public AndCriterion(ICriterion<T> left, ICriterion<T> right)
        {
            _left = left;
            _right = right;
        }

        public Expression HandleExpression(ParameterExpression pe)
        {
            return Expression.AndAlso(_left.HandleExpression(pe),
                _right.HandleExpression(pe));
        }

        public IQueryable<T> HandleQueryable(IQueryable<T> query)
        {
            //var l = _left.HandleQueryable(query).Expression;
            //var r = _right.HandleQueryable(query).Expression;
            //var exp = Expression.AndAlso(l, r);
            //return query.Provider.CreateQuery<T>(exp);
            var left = _left.HandleQueryable(query);
            var right = _right.HandleQueryable(query);
            return left.Intersect(right);
        }
    }
}

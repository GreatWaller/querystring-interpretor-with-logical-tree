using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionTreeLinq
{
    public class NameCriterion : ICriterion<Company>
    {
        private readonly UserCondition _company;

        public NameCriterion(UserCondition company)
        {
            _company = company;
        }

        public Expression HandleExpression(ParameterExpression pe)
        {
            Expression key = Expression.Property(pe, _company.Key);
            Expression value = Expression.Constant(_company.Value);
            Expression exp = Expression.Call(key, typeof(string).GetMethod("Contains", new[] {typeof(string)}), value);

            return exp;
        }


        public IQueryable<Company> HandleQueryable(IQueryable<Company> query)
        {
            return query.Where(p => p.Name.Contains(_company.Value));
        }
    }
}

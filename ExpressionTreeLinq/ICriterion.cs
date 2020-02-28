using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionTreeLinq
{
    public interface ICriterion<T>
    {
        //string HandQueryString(string request);
        Expression HandleExpression(ParameterExpression pe);
        IQueryable<T> HandleQueryable(IQueryable<T> query);
    }
}

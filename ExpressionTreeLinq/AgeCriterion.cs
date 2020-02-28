using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ExpressionTreeLinq
{
    public class AgeCriterion:ICriterion<Company>
    {
        private readonly UserCondition _condition;

        public AgeCriterion(UserCondition condition)
        {
            _condition = condition;
        }

        public Expression HandleExpression(ParameterExpression pe)
        {
            Expression left = Expression.Property(pe, _condition.Key);
            //var property = typeof(Company).GetProperty("Age");
            //Expression exp = GetConvertedSource(pe, property, TypeCode.Int32);

            Expression right = Expression.Constant(Convert.ToInt32(_condition.Value));

            return Expression.Equal(left,right);
        }

        public IQueryable<Company> HandleQueryable(IQueryable<Company> query)
        {
            return query.Where(p => p.Age == Convert.ToInt32(_condition.Value));
        }

        //public static Expression GetConvertedSource(ParameterExpression sourceParameter,
        //                                     PropertyInfo sourceProperty,
        //                                     TypeCode typeCode)
        //{
        //    var sourceExpressionProperty = Expression.Property(sourceParameter,
        //                                                       sourceProperty);

        //    var changeTypeCall = Expression.Call(typeof(Convert).GetMethod("ChangeType",
        //                                                           new[] { typeof(object),
        //                                                    typeof(TypeCode) }),
        //                                                            sourceExpressionProperty,
        //                                                            Expression.Constant(typeCode)
        //                                                            );

        //    Expression convert = Expression.Convert(changeTypeCall,
        //                                            Type.GetType("System." + typeCode));

        //    var convertExpr = Expression.Condition(Expression.Equal(sourceExpressionProperty,
        //                                            Expression.Constant(null, sourceProperty.PropertyType)),
        //                                            Expression.Default(Type.GetType("System." + typeCode)),
        //                                            convert);


        //    return convertExpr;
        //}
    }
}

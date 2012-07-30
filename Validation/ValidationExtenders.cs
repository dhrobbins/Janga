using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace Janga.Validation
{
    public enum Compare
    {
        Equal = ExpressionType.Equal,
        NotEqual = ExpressionType.NotEqual,
        LessThan = ExpressionType.LessThan,
        GreaterThan = ExpressionType.GreaterThan,
        LessThanOrEqual = ExpressionType.LessThanOrEqual,
        GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
        Contains = ExpressionType.TypeIs + 1,
        In = ExpressionType.TypeIs + 2
    }
    
    public static class ValidationExtenders
    {
        public static Validation<T> When<T>(this Validation<T> item,
                                                            string propertyName,
                                                            Compare compareTo,
                                                            object propertyValue)
        {
            //  Determine type of parameter
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            
            //  Property on object to compare with
            Expression property = Expression.Property(parameter, propertyName);
            
            //  The propertyValue to match
            Expression constant = Expression.Constant(propertyValue, propertyValue.GetType());

            //  The type of comparison to make - =, >=, etc
            Expression equality = CreateComparisonExpression<T>(property, compareTo, constant);

            Expression<Func<T, bool>> predicate =
                Expression.Lambda<Func<T, bool>>(equality, parameter);

            var executeDelegate = predicate.Compile();
            item.IsValid = executeDelegate(item.Value);

            //  Report Error handling
            if(item.IsValid == false)
            {
                if(item.ProceedOnFailure)
                {
                    item.ErrorMessages.Add("When " + item.ArgName + "." 
                                                + propertyName + " " + compareTo.ToString()
                                                + " " + propertyValue + " failed.");
                }
                else
                {
                    throw new ArgumentException("When " + item.ArgName + "." 
                                                + propertyName + " " + compareTo.ToString()
                                                + " " + propertyValue + " failed.");
                }
            }

            return item;
        }

        public static Expression CreateComparisonExpression<T>(Expression left, Compare comparesTo, Expression right)
        {
            switch (comparesTo)
            {
                case Compare.Equal:
                    return Expression.Equal(left, right);
                
                case Compare.GreaterThan:
                    return Expression.GreaterThan(left, right);
                
                case Compare.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(left, right);
                
                case Compare.LessThan:
                    return Expression.LessThan(left, right);
                
                case Compare.LessThanOrEqual:
                    return Expression.LessThanOrEqual(left, right);
                
                case Compare.Contains:
                    //  use String.Contains
                    MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    return Expression.Call(left, contains, right);                

                case Compare.NotEqual:
                    return Expression.NotEqual(left, right);

                case Compare.In:                 
                    return Expression.Call(typeof(Enumerable), "Contains", new Type[] { left.Type }, right, left);
                    
                default:
                    throw new ArgumentException("Extensions.CreateComparisonExpression - comparison not supported");

            }
        }
    }

}

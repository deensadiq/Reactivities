using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Core
{
    public class DynamicFilter<T>
    {
        public static IEnumerable<T> FilterBy(IEnumerable<T> items, string filterPropertyName, string filterValue, FilterType filterType)
        {
            // Ex.: x => x.Age == 32
            Func<T, bool> constraint = GetFilterConstraint(filterPropertyName, filterValue, filterType);

            var filteredItems = items.Where(constraint);

            return filteredItems;
        }

        private static Func<T, bool> GetFilterConstraint(string filterPropertyName, string filterValue, FilterType filterType)
        {
            // Ex.: x => x.Age == 34

            // x on the left side.
            ParameterExpression param = Expression.Parameter(typeof(T), "x");

            // x.Age
            MemberExpression propValue = Expression.Property(param, filterPropertyName);

            // 34
            ConstantExpression compareToConstant = GetConstant(filterValue, propValue);

            // x.Age == 34
            BinaryExpression comparison;

            switch (filterType)
            {
                case FilterType.EqualTo:
                    comparison = Expression.Equal(propValue, compareToConstant);
                    break;
                case FilterType.LessThan:
                    comparison = Expression.LessThan(propValue, compareToConstant);
                    break;
                case FilterType.GreaterThan:
                    comparison = Expression.GreaterThan(propValue, compareToConstant);
                    break;
                case FilterType.LessThanOrEqual:
                    comparison = Expression.LessThanOrEqual(propValue, compareToConstant);
                    break;
                case FilterType.GreaterThanOrEqual:
                    comparison = Expression.GreaterThanOrEqual(propValue, compareToConstant);
                    break;
            }

            comparison = Expression.Equal(propValue, compareToConstant);

            LambdaExpression compareEpx = Expression.Lambda(comparison, param);

            var constraint = (Func<T, bool>)compareEpx.Compile();

            return constraint;
        }

        private static ConstantExpression GetConstant(string filterValue, MemberExpression propValue)
        {
            ConstantExpression compareToConstant;

            if (propValue.Type == typeof(int))
            {
                var parkedFilterValue = int.Parse(filterValue);
                compareToConstant = Expression.Constant(parkedFilterValue);
            }
            else if (propValue.Type == typeof(string))
            {
                compareToConstant = Expression.Constant(filterValue);
            }
            else
            {
                throw new NotSupportedException("Filter Value Type is not supported");
            }

            return compareToConstant;
        }
    }
}
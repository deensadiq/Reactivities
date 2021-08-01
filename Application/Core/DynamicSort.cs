using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Core
{
    public static class DynamicSort<T>
    {
        public static IEnumerable<T> SortBy(IEnumerable<T> items, string sortPropertyName, string sortOrder)
        {
            // TODO: Argument vlidation

            // Ex.: x => x.Age
            Func<T, object> sortSelector = GetSortSelector(sortPropertyName);

            var sortedResult = SortAscOrDesc(items, sortSelector, sortOrder);

            return sortedResult;
        }

        private static Func<T, object> GetSortSelector(string sortPropertyName)
        {
            // Ex.: x => x.Age

            // x on the left side
            ParameterExpression param = Expression.Parameter(typeof(T), "x");

            // a.Age
            MemberExpression prop = Expression.Property(param, sortPropertyName);

            UnaryExpression propAsObj = Expression.Convert(prop, typeof(object));

            // x => x.Age
            LambdaExpression sortSelectorExp = Expression.Lambda(propAsObj, param);

            var sortSelector = (Func<T, object>)sortSelectorExp.Compile();

            return sortSelector;
        }

        private static IEnumerable<T> SortAscOrDesc(IEnumerable<T> items, Func<T, object> sortSelector, string sortOrder)
        {
            var sorted = Enumerable.Empty<T>();

            if (sortOrder.ToLower() == "asc")
            {
                sorted = items.OrderBy(sortSelector);
            }
            else if (sortOrder.ToLower() == "desc")
            {
                sorted = items.OrderByDescending(sortSelector);
            }

            return sorted;
        }
    }
}
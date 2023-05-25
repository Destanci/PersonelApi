using System.Linq.Expressions;

namespace PersonelApi.Core.Extensions
{
    public static class LinqExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propetyName)
        {
            return queryable.OrderBy(propetyName.ToLambda<T>());
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> queryable, string propetyName)
        {
            return queryable.OrderByDescending(propetyName.ToLambda<T>());
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> queryable, string propetyName)
        {
            return queryable.ThenBy(propetyName.ToLambda<T>());
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> queryable, string propetyName)
        {
            return queryable.ThenByDescending(propetyName.ToLambda<T>());
        }

        public static Expression<Func<T, object>> ToLambda<T>(this string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propObject, parameter);
        }
    }
}

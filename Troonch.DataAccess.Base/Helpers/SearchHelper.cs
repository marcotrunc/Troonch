using System.Linq.Expressions;
using System.Reflection;

namespace Troonch.DataAccess.Base.Helpers;

public static class SearchHelper
{
    public static IQueryable<T> SearchEntities<T>(this IQueryable<T> source, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return source;
        }

        var parameter = Expression.Parameter(typeof(T), searchTerm);
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        Expression orExpression = null;

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(string))
            {
                var propertyExpression = Expression.Property(parameter, property);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var searchTermExpression = Expression.Constant(searchTerm, typeof(string));
                var containsExpression = Expression.Call(propertyExpression, containsMethod, searchTermExpression);

                if (orExpression == null)
                {
                    orExpression = containsExpression;
                }
                else
                {
                    orExpression = Expression.OrElse(orExpression, containsExpression);
                }
            }
        }

        if (orExpression == null)
        {
            return source;
        }

        var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);
        return source.Where(lambda);
    }
}

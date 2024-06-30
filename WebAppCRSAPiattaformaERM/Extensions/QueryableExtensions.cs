namespace MinimalSPAwithAPIs.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilter<T, TFilter>(IQueryable<T> query, TFilter filter)
    {
        var filterProperties = typeof(TFilter)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.DeclaringType == typeof(TFilter));

        foreach (var prop in filterProperties)
        {
            var filterValue = prop.GetValue(filter);
            if (filterValue != null && filterValue.ToString() != "PageNumber" && filterValue.ToString() != "PageSize" && filterValue.ToString() != "OrderAscDesc" && filterValue.ToString() != "OrderColumnName")
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, prop.Name);
                var constant = Expression.Constant(filterValue);

                Expression body;
                if (prop.PropertyType == typeof(string))
                {
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                    body = Expression.Call(property, containsMethod, constant);
                }
                else if (prop.PropertyType == typeof(DateTime?))
                {
                    var value = (DateTime?)filterValue;
                    if (value.HasValue)
                    {
                        var propertyDate = Expression.Property(property, "Date");
                        var constantDate = Expression.Constant(value.Value.Date, typeof(DateTime));
                        body = Expression.Equal(propertyDate, constantDate);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    body = Expression.Equal(property, constant);
                }

                var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
                query = query.Where(predicate);
            }
        }
        return query;
    }
}
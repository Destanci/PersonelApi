using PersonelApi.Models.ApiModels;
using System.Linq.Expressions;
using System.Reflection;

namespace PersonelApi.Core.Extensions
{
    public static class QueryExtensions
    {
        public static IOrderedQueryable<T> GenerateDataOrder<T>(
            this IQueryable<T> query,
            ICollection<DataRequestOrder> orders)
            where T : class
        {
            IOrderedQueryable<T> orderedQuery = query.OrderBy(x => 0);
            try
            {
                foreach(DataRequestOrder order in orders)
                {
                    if(order.Column != null && order.Dir != null)
                    {
                        var propetyInfo = typeof(T).GetProperty(order.Column, 
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                        if (propetyInfo != null)
                        {
                            if(order.Dir.ToLower().Contains("asc"))
                            {
                                orderedQuery = orderedQuery.ThenBy(propetyInfo.Name);
                            }
                            else
                            {
                                orderedQuery = orderedQuery.ThenByDescending(propetyInfo.Name);
                            }
                        }

                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return query.OrderBy(x => 0);
            }
            return orderedQuery;
        }
    }
}

using System.Linq.Expressions;
using System.Security.Principal;

namespace PersonelApi.Core.DataAccess
{
    public interface IEntityRepository<T>
         where T : class, IEntity, new()
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);

        T? Get(Expression<Func<T, bool>> filter);
        List<T> GetList(Expression<Func<T, bool>> filter);
    }
}

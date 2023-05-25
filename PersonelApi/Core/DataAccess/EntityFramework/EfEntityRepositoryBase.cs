using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PersonelApi.Core.DataAccess.EntityFramework
{
    public abstract class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        protected DbContext Context { get; set; }

        public EfEntityRepositoryBase(DbContext dbContext) { this.Context = dbContext; }

        public TEntity Add(TEntity entity)
        {
            var addedEntity = Context.Entry(entity);
            addedEntity.State = EntityState.Added;
            Context.SaveChanges();
            return entity;
        }

        public void Delete(TEntity entity)
        {
            var updatedEntity = Context.Entry(entity);
            updatedEntity.State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            var updatedEntity = Context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            Context.SaveChanges();
            return entity;
        }


        public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>()
                .AsNoTracking()
                .SingleOrDefault(filter);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>()
                .Where(filter)
                .AsNoTracking()
                .ToList();
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>()
                .Where(filter)
                .Count();
        }
    }
}

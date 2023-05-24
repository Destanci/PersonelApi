using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PersonelApi.Core.DataAccess.EntityFramework
{
    public abstract class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        protected DbContext dbContext { get; set; }

        public EfEntityRepositoryBase(DbContext dbContext) { this.dbContext = dbContext; }

        public TEntity Add(TEntity entity)
        {
            var addedEntity = dbContext.Entry(entity);
            addedEntity.State = EntityState.Added;
            dbContext.SaveChanges();
            return entity;
        }

        public void Delete(TEntity entity)
        {
            var updatedEntity = dbContext.Entry(entity);
            updatedEntity.State = EntityState.Deleted;
            dbContext.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            var updatedEntity = dbContext.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            dbContext.SaveChanges();
            return entity;
        }


        public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
            return dbContext.Set<TEntity>()
                .AsNoTracking()
                .SingleOrDefault(filter);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
        {
            return dbContext.Set<TEntity>()
                .Where(filter)
                .AsNoTracking()
                .ToList();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sprout.Exam.DataAccess.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly SproutExamDbContext dbContext;

        public BaseRepository(SproutExamDbContext dbContext) => this.dbContext = dbContext;

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null)
        {
            var entities = dbContext.Set<TEntity>() as IQueryable<TEntity>;

            if (predicate != null)
            {
                entities = entities.Where(predicate);
            }

            return entities;
        }

        public Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate) => dbContext.Set<TEntity>().SingleOrDefaultAsync(predicate);

        public async Task<TEntity> Add(TEntity entity)
        {
            var entityEntry = dbContext.Set<TEntity>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var entityEntry = dbContext.Set<TEntity>().Update(entity);
            await dbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public Task Delete(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            return dbContext.SaveChangesAsync();
        }
    }
}

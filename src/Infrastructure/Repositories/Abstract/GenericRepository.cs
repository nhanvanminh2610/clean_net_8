using Domain.Repositories.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Abstract
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _dbContext;
        protected DbSet<T> dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(ApplicationDbContext dbContext = null)
        {
            _dbContext = dbContext;
            dbSet = _dbContext.Set<T>();
        }

        public virtual IQueryable<T> AsQueryable() => dbSet.AsNoTracking();

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => AsQueryable().FirstOrDefaultAsync(predicate, cancellationToken);
        
        public Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
            => AsQueryable().Where(predicate).Select(selector).FirstOrDefaultAsync(cancellationToken);

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) 
            => AsQueryable().Where(predicate).ToListAsync();

        public Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default) 
            => AsQueryable().Where(predicate).Select(selector).ToListAsync();

        public async Task AddAsync(T entity, string id, CancellationToken cancellationToken = default)
        {
            UpdateFieldMethorAddEntityModel(entity, id);
            await dbSet.AddAsync(entity);
            return;
        }
        
        public async Task AddAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                UpdateFieldMethorAddEntityModel(entity, id);
            }
            await dbSet.AddRangeAsync(entities);
            return;
        }
        
        public Task UpdateAsync(T entity, string id, CancellationToken cancellationToken = default)
        {
            UpdateFieldMethorUpdateEntityModel(entity, id);
            dbSet.Update(entity);
            return Task.CompletedTask;
        }
        
        public Task UpdateAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                UpdateFieldMethorUpdateEntityModel(entity, id);
            }
            dbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(T entity, string id, CancellationToken cancellationToken = default)
        {
            dbSet.Remove(entity);
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default)
        {
            dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        private void UpdateFieldMethorUpdateEntityModel(T entity, string userId)
        {
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "LastUpdatedBy")
                {
                    property.SetValue(entity, userId);
                }
                if (property.Name == "LastUpdatedTime")
                {
                    property.SetValue(entity, DateTime.UtcNow);
                }
            }
        }
        private void UpdateFieldMethorAddEntityModel(T entity, string userId)
        {
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "CreatedBy")
                {
                    property.SetValue(entity, userId);
                }
                if (property.Name == "CreatedTime")
                {
                    property.SetValue(entity, DateTime.UtcNow);
                }
            }
            UpdateFieldMethorUpdateEntityModel(entity, userId);
        }
    }
}

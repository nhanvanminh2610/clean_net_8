using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _dbContext;
        protected DbSet<T> _dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(ApplicationDbContext dbContext = null)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual IQueryable<T> AsQueryable() => _dbSet.AsNoTracking();
        public virtual IQueryable<T> AsQueryable<T>() where T : class => _dbContext.Set<T>().AsNoTracking();

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => AsQueryable().FirstOrDefaultAsync(predicate, cancellationToken);

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> includeExpression, CancellationToken cancellationToken = default)
            => AsQueryable().Where(predicate).Include(includeExpression).FirstOrDefaultAsync(cancellationToken);

        public Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default) where TResult : class
            => AsQueryable().Where(predicate).Select(selector).FirstOrDefaultAsync(cancellationToken);

        public Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Expression<Func<T, object>> includeExpression, CancellationToken cancellationToken = default) where TResult : class
           => AsQueryable().Where(predicate).Include(includeExpression).Select(selector).FirstOrDefaultAsync(cancellationToken);

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => AsQueryable().Where(predicate).ToListAsync();

        public Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default) where TResult : class
            => AsQueryable().Where(predicate).Select(selector).ToListAsync();

        public async Task AddAsync(T entity, string id, CancellationToken cancellationToken = default)
        {
            UpdateFieldMethorAddEntityModel(entity, id);
            await _dbSet.AddAsync(entity);
            return;
        }

        public async Task AddAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                UpdateFieldMethorAddEntityModel(entity, id);
            }
            await _dbSet.AddRangeAsync(entities);
            return;
        }

        public Task UpdateAsync(T entity, string id, CancellationToken cancellationToken = default)
        {
            UpdateFieldMethorUpdateEntityModel(entity, id);
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                UpdateFieldMethorUpdateEntityModel(entity, id);
            }
            _dbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(entities);
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

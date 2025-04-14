using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();

        Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> includeExpression, CancellationToken cancellationToken = default);

        Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default) where TResult : class;

        Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Expression<Func<T, object>> includeExpression, CancellationToken cancellationToken = default) where TResult : class;

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default) where TResult : class;

        Task AddAsync(T entity, string id, CancellationToken cancellationToken = default);

        Task AddAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, string id, CancellationToken cancellationToken = default);

        Task UpdateAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}

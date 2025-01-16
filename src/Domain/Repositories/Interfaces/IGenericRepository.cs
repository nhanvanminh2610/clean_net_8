using System.Linq.Expressions;

namespace Domain.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();

        Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, string id, CancellationToken cancellationToken = default);

        Task AddAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, string id, CancellationToken cancellationToken = default);

        Task UpdateAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, string id, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<T> entities, string id, CancellationToken cancellationToken = default);
    }
}

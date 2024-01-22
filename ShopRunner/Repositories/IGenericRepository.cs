using ShopRunner.Entities;
using System.Linq.Expressions;

namespace ShopRunner.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetRangeAsync(Expression<Func<T, bool>>? predicate = null, List<string>? includedProps = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<T?> GetByIdAsync(object id);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, List<string>? includedProps = null);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        void Update(T entity);
        Task SaveAsync();
        void Remove(T entity);
        void RemoveRange(List<T> entities);
    }
}

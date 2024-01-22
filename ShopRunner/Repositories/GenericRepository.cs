using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopRunner.DatabaseContexts;
using ShopRunner.Entities;
using System.Linq.Expressions;

namespace ShopRunner.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ShopContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ShopContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetRangeAsync(Expression<Func<T, bool>>? filter = null, List<string>? includedProps = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter is not null)
        {
            query = query.Where(filter);
        }
        if (includedProps is not null)
        {
            foreach (var prop in includedProps)
            {
                query = query.Include(prop);
            }
        }
        if (orderBy is not null)
        {
            return await orderBy(query).AsNoTracking().ToListAsync();
        }
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, List<string>? includedProps = null)
    {
        IQueryable<T> query = _dbSet;
        query = query.Where(predicate);
        if (includedProps is not null)
        {
            foreach (var prop in includedProps)
            {
                query = query.Include(prop);
            }
        }
        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.CountAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(List<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(List<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}

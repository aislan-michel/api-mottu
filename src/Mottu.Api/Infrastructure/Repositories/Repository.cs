using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Infrastructure.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    private readonly AppDbContext _context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<bool> Exists(Expression<Func<T, bool>> condition)
    {
        return await _dbSet.AnyAsync(condition);
    }

    public async Task<T?> GetFirst(Expression<Func<T, bool>> whereCondition)
    {
        if (whereCondition == null)
        {
            throw new ArgumentNullException(nameof(whereCondition));
        }

        return await _dbSet.FirstOrDefaultAsync(whereCondition);
    }

    public async Task<IEnumerable<T>> GetCollection(Expression<Func<T, bool>>? whereCondition = null)
    {
        return whereCondition == null
            ? await _dbSet.ToListAsync()
            : await _dbSet.Where(whereCondition).ToListAsync();
    }

    public async Task Create(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Infrastructure.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    private readonly AppDbContext _context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public bool Exists(Expression<Func<T, bool>> condition)
    {
        return _dbSet.Any(condition);
    }

    public T? GetFirst(Expression<Func<T, bool>> whereCondition)
    {
        if (whereCondition == null)
            throw new ArgumentNullException(nameof(whereCondition));

        return _dbSet.FirstOrDefault(whereCondition);
    }

    public async Task<IEnumerable<T>> GetCollection(Expression<Func<T, bool>>? whereCondition = null)
    {
        return whereCondition == null
            ? await _dbSet.ToListAsync()
            : await _dbSet.Where(whereCondition).ToListAsync();
    }

    public void Create(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
}

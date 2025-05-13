using System.Linq.Expressions;

namespace Mottu.Api.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetFirst(Expression<Func<T, bool>> whereCondition);
    Task<IEnumerable<T>> GetCollection(Expression<Func<T, bool>>? whereCondition = null);
    Task<bool> Exists(Expression<Func<T, bool>> condition);
    Task Create(T t);
    Task Update(T t);
    Task Delete(T t);
}

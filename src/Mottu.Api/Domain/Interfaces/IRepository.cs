using System.Linq.Expressions;

namespace Mottu.Api.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    T? GetFirst(Expression<Func<T, bool>> whereCondition);
    IEnumerable<T> GetCollection(Expression<Func<T, bool>>? whereCondition = null);
    bool Exists(Expression<Func<T, bool>> condition);
    void Create(T t);
    void Update(T t);
    void Delete(T t);
}

namespace Mottu.Api.Infrastructure.Repositories.GenericRepository;

public interface IRepository<T>
{
    T GetFirst(Func<T, bool> whereCondition);
    IEnumerable<T> GetCollection(Func<T, bool>? whereCondition = null);
    bool Exists(Predicate<T> condition);
    void Save(T t);
    void Update(T t);
}
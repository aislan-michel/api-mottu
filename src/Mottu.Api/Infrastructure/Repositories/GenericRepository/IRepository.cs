namespace Mottu.Api.Infrastructure.Repositories.GenericRepository;

public interface IRepository<T>
{
    IEnumerable<T> Get(Func<T, bool>? whereCondition = null);
    bool Exists(Predicate<T> condition);
    void Save(T t);
}
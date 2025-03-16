namespace Mottu.Api.Infrastructure.Repositories.GenericRepository;

public interface IRepository<T>
{
    bool Exists(Predicate<T> condition);
    void Save(T t);
}
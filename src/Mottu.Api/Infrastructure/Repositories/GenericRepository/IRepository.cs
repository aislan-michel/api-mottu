namespace Mottu.Api.Infrastructure.Repositories.GenericRepository;

public interface IRepository<T>
{
    void Save(T t);
}
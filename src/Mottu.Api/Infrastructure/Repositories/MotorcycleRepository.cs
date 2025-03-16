using Mottu.Api.Infrastructure.Repositories.GenericRepository;

namespace Mottu.Api.Infrastructure.Repositories;

public class Repository<Motorcycle> : IRepository<Motorcycle>
{
    private static readonly List<Motorcycle> Motorcycles = [];

    public void Save(Motorcycle motorcycle)
    {
        Motorcycles.Add(motorcycle);
    }
}
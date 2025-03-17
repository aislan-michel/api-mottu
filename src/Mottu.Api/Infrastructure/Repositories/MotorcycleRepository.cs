

using Mottu.Api.Infrastructure.Repositories.GenericRepository;

namespace Mottu.Api.Infrastructure.Repositories;

public class Repository<Motorcycle> : IRepository<Motorcycle>
{
    private static readonly List<Motorcycle> Motorcycles = [];

    public bool Exists(Predicate<Motorcycle> condition)
    {
        return Motorcycles.Exists(condition);
    }

    public IEnumerable<Motorcycle> Get(Func<Motorcycle, bool>? whereCondition = null)
    {
        if(whereCondition == null)
        {
            return Motorcycles;
        }

        return Motorcycles.Where(whereCondition);
    }

    public void Save(Motorcycle motorcycle)
    {
        Motorcycles.Add(motorcycle);
    }
}
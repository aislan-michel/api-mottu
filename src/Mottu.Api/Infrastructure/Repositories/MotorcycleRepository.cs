using Mottu.Api.Infrastructure.Repositories.GenericRepository;

namespace Mottu.Api.Infrastructure.Repositories;

public class Repository<Motorcycle> : IRepository<Motorcycle>
{
    private static readonly List<Motorcycle> Motorcycles = [];

    public bool Exists(Predicate<Motorcycle> condition)
    {
        return Motorcycles.Exists(condition);
    }

    public Motorcycle GetFirst(Func<Motorcycle, bool> whereCondition)
    {
        if(whereCondition == null)
        {
            throw new ArgumentNullException(nameof(whereCondition), "Condição para pegar o primeiro item não pode ser nulo");
        }

        return Motorcycles.First(whereCondition);
    }

    public IEnumerable<Motorcycle> GetCollection(Func<Motorcycle, bool>? whereCondition = null)
    {
        return whereCondition == null ? Motorcycles : Motorcycles.Where(whereCondition);
    }

    public void Create(Motorcycle motorcycle)
    {
        Motorcycles.Add(motorcycle);
    }

    public void Update(Motorcycle motorcycle)
    {
        Motorcycles.Remove(motorcycle);

        Motorcycles.Add(motorcycle);
    }

    public void Delete(Motorcycle motorcycle)
    {
        Motorcycles.Remove(motorcycle);
    }
}
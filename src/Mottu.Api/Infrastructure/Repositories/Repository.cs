using Mottu.Api.Domain.Interfaces;

namespace Mottu.Api.Infrastructure.Repositories;

public class Repository<T> : IRepository<T>
{
    private static readonly List<T> Items = [];

    public bool Exists(Predicate<T> condition)
    {
        return Items.Exists(condition);
    }

    public T? GetFirst(Func<T, bool> whereCondition)
    {
        if(whereCondition == null)
        {
            throw new ArgumentNullException(nameof(whereCondition), "Condição para pegar o primeiro item não pode ser nulo");
        }

        return Items.FirstOrDefault(whereCondition);
    }

    public IEnumerable<T> GetCollection(Func<T, bool>? whereCondition = null)
    {
        return whereCondition == null ? Items : Items.Where(whereCondition);
    }

    public void Create(T motorcycle)
    {
        Items.Add(motorcycle);
    }

    public void Update(T motorcycle)
    {
        Items.Remove(motorcycle);

        Items.Add(motorcycle);
    }

    public void Delete(T motorcycle)
    {
        Items.Remove(motorcycle);
    }
}
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;

namespace Mottu.Api.Application.Interfaces;

public interface IUnitOfWork
{
    IRepository<Motorcycle> Motorcycles { get; }
    IRepository<DeliveryMan> DeliveryMen { get; }
    IRepository<Rent> Rents { get; }

    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
    Task Save();
    void Dispose();
}
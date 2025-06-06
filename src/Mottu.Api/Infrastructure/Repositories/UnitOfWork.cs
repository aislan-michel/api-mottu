
using Microsoft.EntityFrameworkCore.Storage;

using Mottu.Api.Application.Interfaces;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Infrastructure.Repositories;

public class UnitOfWork(
    AppDbContext appDbContext,
    IRepository<Motorcycle> motorcycleRepository,
    IRepository<DeliveryMan> deliveryManRepository,
    IRepository<Rent> rentRepository) : IUnitOfWork
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private IDbContextTransaction? _transaction;

    private readonly IRepository<Motorcycle> _motorcycleRepository = motorcycleRepository;
    private readonly IRepository<DeliveryMan> _deliveryManRepository = deliveryManRepository;
    private readonly IRepository<Rent> _rentRepository = rentRepository;

    public IRepository<Motorcycle> Motorcycles => _motorcycleRepository;

    public IRepository<DeliveryMan> DeliveryMen => _deliveryManRepository;

    public IRepository<Rent> Rents => _rentRepository;

    public IDbContextTransaction? Transaction => _transaction;

    public async Task BeginTransaction()
    {
        if (_transaction == null)
        {
            _transaction = await _appDbContext.Database.BeginTransactionAsync();
        }
    }

    public async Task CommitTransaction()
    {
        if (_transaction != null)
        {
            await _appDbContext.SaveChangesAsync();

            await _transaction.CommitAsync();

            await _transaction.DisposeAsync();

            _transaction = null;
        }
    }

    public async Task RollbackTransaction()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();

            await _transaction.DisposeAsync();

            _transaction = null;
        }
    }

    public async Task Save()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _appDbContext.Dispose();
    }

}
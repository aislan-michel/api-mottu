using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Application.Models;
using FluentValidation;
using System.Text.Json;
using Mottu.Api.Extensions;

namespace Mottu.Api.Application.UseCases.RentUseCases;

public class RentUseCase : IRentUseCase
{
    private readonly IRepository<Rent> _rentRepository;
    private readonly IRepository<DeliveryMan> _deliveryManRepository;
    private readonly IRepository<Motorcycle> _motorcycleRepository;
    private readonly IValidator<PostRentRequest> _postRentRequestValidator;
    private readonly IValidator<PatchRentRequest> _patchRentRequestValidator;
    private readonly ILogger<RentUseCase> _logger;

    public RentUseCase(
        IRepository<Rent> rentRepository, 
        IRepository<DeliveryMan> deliveryManRepository, 
        IRepository<Motorcycle> motorcycleRepository, 
        IValidator<PostRentRequest> postRentRequestValidator, 
        IValidator<PatchRentRequest> patchRentRequestValidator, 
        ILogger<RentUseCase> logger)
    {
        _rentRepository = rentRepository;
        _deliveryManRepository = deliveryManRepository;
        _motorcycleRepository = motorcycleRepository;
        _postRentRequestValidator = postRentRequestValidator;
        _patchRentRequestValidator = patchRentRequestValidator;
        _logger = logger;
    }

    public Result<CreateRentResponse> Create(PostRentRequest request)
    {
        var validationResult = _postRentRequestValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Result<CreateRentResponse>.Fail(validationResult.GetErrorMessages());
        }

        var deliveryMan = _deliveryManRepository.GetFirst(x => x.Id == request.DeliveryManId);

        if(deliveryMan == null)
        {
            return Result<CreateRentResponse>.Fail("Entregador não encontrado");
        }

        if(!deliveryMan.DriverLicense.TypeIsA())
        {
            return Result<CreateRentResponse>.Fail("Tipo da CNH do entregador é diferente de A");
        }

        var motorcycle = _motorcycleRepository.GetFirst(x => x.Id == request.MotorcycleId);

        if(motorcycle == null)
        {
            return Result<CreateRentResponse>.Fail("Moto não encontrada");
        }
        
        var rent = new Rent(deliveryMan, motorcycle, new Plan(request.Plan));
        
        _logger.LogInformation("create a rent... rent: {rent}", JsonSerializer.Serialize(rent));

        _rentRepository.Create(rent);

        return Result<CreateRentResponse>.Ok(new CreateRentResponse());
    }

    public Result<GetRentResponse?> GetById(string id)
    {
        var rent = _rentRepository.GetFirst(x => x.Id == x.Id);

        if(rent == null)
        {
            return Result<GetRentResponse?>.Fail("locação não encontrada");
        }

        return Result<GetRentResponse?>.Ok(new GetRentResponse(
            rent.Id, rent.Plan.DailyRate,
            rent.DeliveryMan.Id, rent.Motorcycle.Id, 
            rent.StartDate, rent.EndDate, rent.ExpectedEndDate,
            rent.ReturnDate, rent.TotalAmountPayable));
    }

    public Result<UpdateRentResponse> Update(string id, PatchRentRequest request)
    {
        var validationResult = _patchRentRequestValidator.Validate(request);

        if(!validationResult.IsValid)
        {
            return Result<UpdateRentResponse>.Fail(validationResult.GetErrorMessages());
        }

        var rent = _rentRepository.GetFirst(x => x.Id == id);

        if(rent == null)
        {
            return Result<UpdateRentResponse>.Fail($"Locação com id {id} não encontrada");
        }

        rent.UpdateReturnDate(request.ReturnDate);
        rent.SetTotalAmountPayable();

        _rentRepository.Update(rent);

        return Result<UpdateRentResponse>.Ok(new UpdateRentResponse());
    }

    public IEnumerable<GetRentResponse> Get()
    {
        return _rentRepository.GetCollection().Select(rent => new GetRentResponse(
            rent.Id, rent.Plan.DailyRate,
            rent.DeliveryMan.Id, rent.Motorcycle.Id, 
            rent.StartDate, rent.EndDate, rent.ExpectedEndDate,
            rent.ReturnDate, rent.TotalAmountPayable));
    }
}
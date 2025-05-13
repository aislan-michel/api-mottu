using Mottu.Api.Domain.Entities;
using Mottu.Api.Application.Models;
using FluentValidation;
using System.Text.Json;
using Mottu.Api.Extensions;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Application.UseCases;

public class RentUseCase(
    IRepository<Rent> rentRepository,
    IRepository<DeliveryMan> deliveryManRepository,
    IRepository<Motorcycle> motorcycleRepository,
    IValidator<PostRentRequest> postRentRequestValidator,
    IValidator<PatchRentRequest> patchRentRequestValidator,
    ILogger<RentUseCase> logger,
    ILoggedUserService loggedUserService) : IRentUseCase
{
    private readonly IRepository<Rent> _rentRepository = rentRepository;
    private readonly IRepository<DeliveryMan> _deliveryManRepository = deliveryManRepository;
    private readonly IRepository<Motorcycle> _motorcycleRepository = motorcycleRepository;
    private readonly IValidator<PostRentRequest> _postRentRequestValidator = postRentRequestValidator;
    private readonly IValidator<PatchRentRequest> _patchRentRequestValidator = patchRentRequestValidator;
    private readonly ILogger<RentUseCase> _logger = logger;
    private readonly ILoggedUserService _loggedUserService = loggedUserService;

    public async Task<Result<string>> Create(PostRentRequest request)
    {
        var validationResult = _postRentRequestValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Result<string>.Fail(validationResult.GetErrorMessages());
        }

        var deliveryManId = _loggedUserService.DeliveryManId;

        var deliveryMan = await _deliveryManRepository.GetFirst(x => x.Id == deliveryManId);

        if(deliveryMan == null)
        {
            return Result<string>.Fail("Entregador não encontrado");
        }

        if(!deliveryMan.DriverLicense.TypeIsA())
        {
            return Result<string>.Fail("Tipo da CNH do entregador é diferente de A");
        }

        var motorcycle = await _motorcycleRepository.GetFirst(x => x.Id == request.MotorcycleId);

        if(motorcycle == null)
        {
            return Result<string>.Fail("Moto não encontrada");
        }
        
        var rent = new Rent(deliveryMan, motorcycle, new Plan(request.Plan));
        
        _logger.LogInformation("create a rent... rent: {rent}", JsonSerializer.Serialize(rent));

        await _rentRepository.Create(rent);

        return Result<string>.Ok(string.Empty);
    }

    public async Task<Result<GetRentResponse?>> GetById(string id)
    {
        var rent = await _rentRepository.GetFirst(x => x.Id == x.Id);

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

    public async Task<Result<string>> Update(string id, PatchRentRequest request)
    {
        var validationResult = _patchRentRequestValidator.Validate(request);

        if(!validationResult.IsValid)
        {
            return Result<string>.Fail(validationResult.GetErrorMessages());
        }

        var rent = await _rentRepository.GetFirst(x => x.Id == id);

        if(rent == null)
        {
            return Result<string>.Fail($"Locação com id {id} não encontrada");
        }

        rent.UpdateReturnDate(request.ReturnDate);
        rent.SetTotalAmountPayable();

        await _rentRepository.Update(rent);

        return Result<string>.Ok(string.Empty);
    }

    public async Task<IEnumerable<GetRentResponse>> Get()
    {
        var deliveryManId = _loggedUserService.DeliveryManId;

        var rents = await _rentRepository.GetCollection(x => x.DeliveryManId == deliveryManId);

        return rents.Select(rent => new GetRentResponse(
                rent.Id, rent.Plan.DailyRate,
                rent.DeliveryManId, rent.MotorcycleId, 
                rent.StartDate, rent.EndDate, rent.ExpectedEndDate,
                rent.ReturnDate, rent.TotalAmountPayable));
    }
}
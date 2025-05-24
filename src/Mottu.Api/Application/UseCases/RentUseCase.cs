using Mottu.Api.Domain.Entities;
using Mottu.Api.Application.Models;
using FluentValidation;
using System.Text.Json;
using Mottu.Api.Extensions;
using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Application.UseCases;

public class RentUseCase(
    IUnitOfWork unitOfWork,
    IValidator<PostRentRequest> postRentRequestValidator,
    IValidator<PatchRentRequest> patchRentRequestValidator,
    ILogger<RentUseCase> logger,
    ILoggedUserService loggedUserService) : IRentUseCase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
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

        var deliveryMan = await GetLoggedDeliveryMan();

        if (deliveryMan == null)
        {
            return Result<string>.Fail("Entregador não encontrado");
        }

        if (!deliveryMan.DriverLicense.TypeIsA())
        {
            return Result<string>.Fail("Tipo da CNH do entregador é diferente de A");
        }

        var rent = new Rent(deliveryMan.Id, request.MotorcycleId, new Plan(request.Plan));

        _logger.LogInformation("create a rent... rent: {rent}", JsonSerializer.Serialize(rent));

        try
        {
            await _unitOfWork.BeginTransaction();

            await _unitOfWork.Rents.Create(rent);

            await UpdateMotorcycle(request.MotorcycleId, rent.Id);

            await UpdateDeliveryMan(deliveryMan, rent.Id);

            await _unitOfWork.CommitTransaction();

            return Result<string>.Ok(string.Empty);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    private async Task<DeliveryMan?> GetLoggedDeliveryMan()
    {
        var deliveryManId = _loggedUserService.DeliveryManId;
        return await _unitOfWork.DeliveryMen.GetFirst(x => x.Id == deliveryManId);
    }

    private async Task UpdateMotorcycle(string motorcycleId, string rentId)
    {
        var motorcycle = await _unitOfWork.Motorcycles.GetFirst(x => x.Id == motorcycleId);

        motorcycle!.UpdateRentId(rentId);

        await _unitOfWork.Motorcycles.Update(motorcycle);
    }

    private async Task UpdateDeliveryMan(DeliveryMan deliveryMan, string rentId)
    {
        deliveryMan.UpdateRentId(rentId);

        await _unitOfWork.DeliveryMen.Update(deliveryMan);
    }

    public async Task<Result<GetRentResponse?>> GetById(string id)
    {
        var rent = await _unitOfWork.Rents.GetFirst(x => x.Id == x.Id);

        if (rent == null)
        {
            return Result<GetRentResponse?>.Fail("Locação não encontrada");
        }

        return Result<GetRentResponse?>.Ok(new GetRentResponse(
            rent.Id, rent.Plan.DailyRate,
            rent.DeliveryManId, rent.MotorcycleId,
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

        var rent = await _unitOfWork.Rents.GetFirst(x => x.Id == id);

        if(rent == null)
        {
            return Result<string>.Fail($"Locação com id {id} não encontrada");
        }

        rent.UpdateReturnDate(request.ReturnDate);
        rent.SetTotalAmountPayable();

        await _unitOfWork.Rents.Update(rent);

        return Result<string>.Ok(string.Empty);
    }

    public async Task<IEnumerable<GetRentResponse>> Get()
    {
        var deliveryManId = _loggedUserService.DeliveryManId;

        var rents = await _unitOfWork.Rents.GetCollection(x => x.DeliveryManId == deliveryManId);

        return rents.Select(rent => new GetRentResponse(
                rent.Id, rent.Plan.DailyRate,
                rent.DeliveryManId, rent.MotorcycleId, 
                rent.StartDate, rent.EndDate, rent.ExpectedEndDate,
                rent.ReturnDate, rent.TotalAmountPayable));
    }
}
using Mottu.Api.Domain.Entities;
using Mottu.Api.Application.Models;
using FluentValidation;
using Mottu.Api.Extensions;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Application.UseCases;

public class MotorcycleUseCase(
    IRepository<Motorcycle> motorcycleRepository,
    IRepository<Rent> rentRepository,
    IValidator<PostMotorcycleRequest> postMotorcycleRequestValidator,
    IValidator<PatchMotorcycleRequest> patchMotorcycleRequestValidator) : IMotorcycleUseCase
{
    private readonly IRepository<Motorcycle> _motorcycleRepository = motorcycleRepository;
    private readonly IRepository<Rent> _rentRepository = rentRepository;
    private readonly IValidator<PostMotorcycleRequest> _postMotorcycleRequestValidator = postMotorcycleRequestValidator;
    private readonly IValidator<PatchMotorcycleRequest> _patchMotorcycleRequestValidator = patchMotorcycleRequestValidator;

    public Result<string> Create(PostMotorcycleRequest request)
    {
        var validationResult = _postMotorcycleRequestValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Result<string>.Fail(validationResult.GetErrorMessages());
        }

        var motorcycle = new Motorcycle(request.Year, request.Model, request.Plate);

        _motorcycleRepository.Create(motorcycle);

        //todo: produces event

        return Result<string>.Ok(string.Empty);
    }

    public async Task<IEnumerable<GetMotorcycleResponse>> Get(string? plate)
    {
        var motorcycles = await _motorcycleRepository
            .GetCollection(string.IsNullOrWhiteSpace(plate) ? null : x => x.Plate == plate);

        return motorcycles.Select(x => new GetMotorcycleResponse(x.Id, x.Year, x.Model, x.Plate));
    }

    //todo: create unit tests
    public GetMotorcycleResponse? GetById(string id)
    {
        var motorcycle = _motorcycleRepository.GetFirst(x => x.Id == id);

        if (motorcycle == null)
        {
            return default;
        }

        return new GetMotorcycleResponse(motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.Plate);
    }

    public Result<string> Update(string id, PatchMotorcycleRequest request)
    {
        var validationResult = _patchMotorcycleRequestValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Result<string>.Fail(validationResult.GetErrorMessages());
        }

        var motorcycle = _motorcycleRepository.GetFirst(x => x.Id == id);

        if (motorcycle == null)
        {
            return Result<string>.Fail("Moto não encontrada");
        }

        motorcycle.UpdatePlate(request.Plate);

        _motorcycleRepository.Update(motorcycle);

        return Result<string>.Ok(string.Empty);
    }

    public Result<string> Delete(string id)
    {
        var motorcycle = _motorcycleRepository.GetFirst(x => x.Id == id);

        if (motorcycle == null)
        {
            return Result<string>.Fail("Moto não encontrada");
        }

        var rent = _rentRepository.GetFirst(x => x.Motorcycle.Id == id);

        if (rent != null)
        {
            return Result<string>.Fail($"Moto possui registro de locação, id da locação: {rent.Id}");
        }

        _motorcycleRepository.Delete(motorcycle);

        return Result<string>.Ok(string.Empty);
    }
}
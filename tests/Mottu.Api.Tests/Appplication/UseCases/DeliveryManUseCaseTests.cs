using FluentValidation;
using FluentValidation.Results;

using Moq;

using Mottu.Api.Application.Interfaces;
using Mottu.Api.Application.Models;
using Mottu.Api.Application.UseCases;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;

namespace Mottu.Api.Tests.Application.UseCases;

public class DeliveryManUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly Mock<IValidator<RegisterDeliveryManRequest>> _registerDeliveryManRequestValidatorMock;
    private readonly Mock<IValidator<PatchDriverLicenseImageRequest>> _patchDriverLicenseImageRequestValidatorMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly IDeliveryManUseCase _deliveryManUseCase;

    public DeliveryManUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _storageServiceMock = new Mock<IStorageService>();
        _registerDeliveryManRequestValidatorMock = new Mock<IValidator<RegisterDeliveryManRequest>>();
        _patchDriverLicenseImageRequestValidatorMock = new Mock<IValidator<PatchDriverLicenseImageRequest>>();
        _authServiceMock = new Mock<IAuthService>();

        _deliveryManUseCase = new DeliveryManUseCase(
            _unitOfWorkMock.Object, _storageServiceMock.Object,
            _registerDeliveryManRequestValidatorMock.Object, _patchDriverLicenseImageRequestValidatorMock.Object,
            _authServiceMock.Object
        );
    }

    [Fact]
    public async Task Register_ShouldResultOk()
    {
        var request = new RegisterDeliveryManRequest
        {
            Username = "user.teste",
            Email = "user.teste@email.com",
            Password = "Teste@123",
            Name = "Fulano",
            CompanyRegistrationNumber = "29714323000129",
            DateOfBirth = new DateOnly(1999, 12, 28),
            DriverLicense = "25082389495",
            DriverLicenseType = "B",
            DriverLicenseImageBase64 = default
        };

        _registerDeliveryManRequestValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _unitOfWorkMock.Setup(x => x.BeginTransaction());

        _authServiceMock.Setup(x => x.Register(It.IsAny<RegisterUserRequest>())).ReturnsAsync(Result<RegisterUserResponse>.Ok(new RegisterUserResponse(Guid.NewGuid().ToString())));

        _unitOfWorkMock.Setup(x => x.DeliveryMen.Create(It.IsAny<DeliveryMan>()));

        _unitOfWorkMock.Setup(x => x.CommitTransaction());

        var result = await _deliveryManUseCase.Register(request);

        _registerDeliveryManRequestValidatorMock.Verify(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _authServiceMock.Verify(x => x.Register(It.IsAny<RegisterUserRequest>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.RollbackTransaction(), Times.Never);
        _storageServiceMock.Verify(x => x.SaveBase64Image(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.DeliveryMen.Create(It.IsAny<DeliveryMan>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransaction(), Times.Once);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Empty(result.Messages);
        
    }
}
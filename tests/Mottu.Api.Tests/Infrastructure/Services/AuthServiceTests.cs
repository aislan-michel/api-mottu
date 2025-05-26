using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Moq;

using Mottu.Api.Application.Interfaces;
using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Infrastructure.Identity;
using Mottu.Api.Infrastructure.Interfaces;
using Mottu.Api.Infrastructure.Services;

using Mottu.Api.Tests.Factories;

namespace Mottu.Api.Tests.Infrastructure.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserManager> _userManagerMock;
    private readonly Mock<ISignInManager> _signInManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly Mock<IRepository<DeliveryMan>> _deliveryManRepositoryMock;
    private readonly IAuthService _authService;

    public AuthServiceTests()
    {
        _userManagerMock = new Mock<IUserManager>();
        _signInManagerMock = new Mock<ISignInManager>();
        _tokenServiceMock = new Mock<ITokenService>();
        _loggerMock = new Mock<ILogger<AuthService>>();
        _deliveryManRepositoryMock = new Mock<IRepository<DeliveryMan>>();

        _authService = new AuthService(_userManagerMock.Object, _signInManagerMock.Object,
            _tokenServiceMock.Object, _loggerMock.Object, _deliveryManRepositoryMock.Object);
    }

    [Fact]
    public async Task Register_ShouldResultOk()
    {
        var request = new RegisterUserRequest()
        {
            Username = "user.test",
            Email = "user.test@email.com",
            Password = "Test@123",
            Role = "admin"
        };

        _userManagerMock.Setup(x => x.Create(It.IsAny<ApplicationUser>(), request.Password))
            .ReturnsAsync(Result<string>.Ok(string.Empty));

        _userManagerMock.Setup(x => x.AddToRole(It.IsAny<ApplicationUser>(), request.Role))
            .ReturnsAsync(Result<string>.Ok(string.Empty));

        var result = await _authService.Register(request);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Empty(result.Messages);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.UserId);
        Assert.NotEmpty(result.Data.UserId);
    }

    [Fact]
    public async Task Login_ShouldResultFail_WhenDeliveryManIsInactive()
    {
        var request = new LoginUserRequest()
        {
            Username = "user.test",
            Password = "Test@123",
        };

        _userManagerMock.Setup(x => x.FindByName(request.Username!))
            .ReturnsAsync(Result<ApplicationUser>.Ok(ApplicationUserFactory.ApplicationUser("user.test")));

        _signInManagerMock.Setup(x => x.CheckPasswordSignIn(It.IsAny<ApplicationUser>(), request.Password!))
            .ReturnsAsync(Result<string>.Ok(string.Empty));

        _userManagerMock.Setup(x => x.GetRoles(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(["entregador"]);

        _deliveryManRepositoryMock.Setup(x => x.GetFirst(It.IsAny<Expression<Func<DeliveryMan, bool>>>()))
            .ReturnsAsync(DeliveryManFactory.DeliveryManWithoutDriverLicenseImage());

        var result = await _authService.Login(request);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Single(result.Messages);
        Assert.Contains("usuário não está ativo", result.Messages.First());

        _userManagerMock.Verify(x => x.FindByName(request.Username), Times.Once);
        _signInManagerMock.Verify(x => x.CheckPasswordSignIn(It.IsAny<ApplicationUser>(), request.Password!), Times.Once);
        _userManagerMock.Verify(x => x.GetRoles(It.IsAny<ApplicationUser>()), Times.Once);
        _deliveryManRepositoryMock.Verify(x => x.GetFirst(It.IsAny<Expression<Func<DeliveryMan, bool>>>()), Times.Once);
    }
}
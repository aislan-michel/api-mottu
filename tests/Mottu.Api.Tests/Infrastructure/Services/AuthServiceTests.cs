using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Moq;

using Mottu.Api.Application.Interfaces;
using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Infrastructure.Identity;
using Mottu.Api.Infrastructure.Interfaces;
using Mottu.Api.Infrastructure.Services;

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
}
using Microsoft.AspNetCore.Identity;

using Mottu.Api.Application.Models;
using Mottu.Api.Infrastructure.Identity;
using Mottu.Api.Application.Interfaces;
using Mottu.Api.Infrastructure.Interfaces;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Domain.Entities;

namespace Mottu.Api.Infrastructure.Services;

public class AuthService(
    IUserManager userManager,
    ISignInManager signInManager,
    ITokenService tokenService,
    ILogger<AuthService> logger,
    IRepository<DeliveryMan> deliveryManRepository) : IAuthService
{
    private readonly IUserManager _userManager = userManager;
    private readonly ISignInManager _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IRepository<DeliveryMan> _deliveryManRepository = deliveryManRepository;

    public async Task<Result<RegisterUserResponse>> Register(RegisterUserRequest request)
    {
        //todo: maybe need validate de request or use dtos for params
        var user = new ApplicationUser { UserName = request.Username, Email = request.Email };
        var result = await _userManager.Create(user, request.Password);
        if (!result.Success)
        {
            return Result<RegisterUserResponse>.Fail(result.GetMessages());
        }

        result = await _userManager.AddToRole(user, request.Role);
        if (!result.Success)
        {
            return Result<RegisterUserResponse>.Fail(result.GetMessages());
        }

        return Result<RegisterUserResponse>.Ok(new RegisterUserResponse(user.Id));
    }

    public async Task<Result<string>> Login(LoginUserRequest request)
    {
        var result = await _userManager.FindByName(request.Username!);
        if (!result.Success)
        {
            return Result<string>.Fail("usuário não encontrado");
        }
        var user = result.Data!;

        _logger.LogInformation("user found... username: {username}", user.UserName);

        var resultCheckPassword = await _signInManager.CheckPasswordSignIn(user, request.Password!);
        if (!resultCheckPassword.Success)
        {
            return Result<string>.Fail("usuário não encontrado");
        }

        var roles = await _userManager.GetRoles(user);

        _logger.LogInformation("roles found... roles: {roles}", string.Join(", ", roles));

        var deliveryMan = await _deliveryManRepository.GetFirst(x => x.UserId == user.Id);
        var token = _tokenService.GenerateToken(user, deliveryMan?.Id, roles);

        return Result<string>.Ok(token);
    }
}
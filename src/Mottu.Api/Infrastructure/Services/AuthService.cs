using Microsoft.AspNetCore.Identity;

using Mottu.Api.Application.Models;
using Mottu.Api.Infrastructure.Identity;
using Mottu.Api.Application.Interfaces;
using Mottu.Api.Infrastructure.Interfaces;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Domain.Entities;

namespace Mottu.Api.Infrastructure.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager, 
    SignInManager<ApplicationUser> signInManager,
    ITokenService tokenService,
    ILogger<AuthService> logger,
    IRepository<DeliveryMan> deliveryManRepository) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IRepository<DeliveryMan> _deliveryManRepository = deliveryManRepository;

    public async Task<Result<RegisterUserResponse>> Register(RegisterUserRequest request)
    {
        var user = new ApplicationUser { UserName = request.Username, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result<RegisterUserResponse>.Fail(result.Errors.Select(x => x.Description));
        }

        await _userManager.AddToRoleAsync(user, request.Role);

        return Result<RegisterUserResponse>.Ok(new RegisterUserResponse(user.Id));
    }

    public async Task<Result<string>> Login(LoginUserRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return Result<string>.Fail("usuário não encontrado");
        }

        _logger.LogInformation("user found... username: {username}", user.UserName);

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Result<string>.Fail("usuário não encontrado");
        }

        var roles = await _userManager.GetRolesAsync(user);

        _logger.LogInformation("roles found... roles: {roles}", string.Join(", ", roles));

        string token;
        if(request.Role == Roles.Entregador)
        {
            var deliveryMan = _deliveryManRepository.GetFirst(x => x.UserId == user.Id);

            if(deliveryMan == null)
            {
                return Result<string>.Fail("Entregador não encontrado");
            }

            token = _tokenService.GenerateToken(user, deliveryMan.Id, roles);
            return Result<string>.Ok(token);
        }

        token = _tokenService.GenerateToken(user, null, roles);
        return Result<string>.Ok(token);
    }
}
using Microsoft.AspNetCore.Identity;

using Mottu.Api.Application.Models;
using Mottu.Api.Infrastructure.Identity;
using Mottu.Api.Infrastructure.Interfaces;

namespace Mottu.Api.Infrastructure.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager, 
    SignInManager<ApplicationUser> signInManager,
    ITokenService tokenService) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<string>> Register(RegisterUserRequest request)
    {
        var user = new ApplicationUser { UserName = request.Username, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result<string>.Fail(result.Errors.Select(x => x.Description));
        }

        await _userManager.AddToRoleAsync(user, request.Role);

        return Result<string>.Ok("usuário criado");
    }

    public async Task<Result<string>> Login(LoginUserRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return Result<string>.Fail("usuário não encontrado");
        }
            
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Result<string>.Fail("usuário não encontrado");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _tokenService.GenerateToken(roles);
        return Result<string>.Ok(token);
    }
}
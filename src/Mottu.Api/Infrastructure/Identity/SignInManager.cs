
using Microsoft.AspNetCore.Identity;

using Mottu.Api.Application.Interfaces;
using Mottu.Api.Application.Models;

namespace Mottu.Api.Infrastructure.Identity;

public class SignInManager(SignInManager<ApplicationUser> signInManager) : ISignInManager
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<Result<string>> CheckPasswordSignIn(ApplicationUser user, string password)
    {
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return Result<string>.Fail("usuário não encontrado");
        }

        return Result<string>.Ok(string.Empty);
    }
}

using Microsoft.AspNetCore.Identity;

using Mottu.Api.Infrastructure.Interfaces;
using Mottu.Api.Application.Models;

namespace Mottu.Api.Infrastructure.Identity;

public class UserManager(UserManager<ApplicationUser> userManager) : IUserManager
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<string>> AddToRole(ApplicationUser user, string role)
    {
        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
        {
            return Result<string>.Fail(result.Errors.Select(x => x.Description));
        }

        return Result<string>.Ok(string.Empty);
    }

    public async Task<Result<string>> Create(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return Result<string>.Fail(result.Errors.Select(x => x.Description));
        }

        return Result<string>.Ok(string.Empty);
    }

    public async Task<Result<ApplicationUser>> FindByName(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return Result<ApplicationUser>.Fail("usuário não encontrado");
        }

        return Result<ApplicationUser>.Ok(user);
    }

    public async Task<IList<string>?> GetRoles(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}
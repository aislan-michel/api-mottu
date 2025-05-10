using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Mottu.Api.Application.Models;
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
[Produces("application/json")]
[Authorize(Roles = "admin")]
public class UsersController(UserManager<ApplicationUser> userManager) : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var usersResponse = new List<GetUsersResponse>();
        var users = await _userManager.Users.ToListAsync();
        
        foreach(var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            usersResponse.Add(new GetUsersResponse(user.UserName, user.Email, string.Join(", ", roles)));
        }

        return Ok(Result<List<GetUsersResponse>>.Ok(usersResponse));
    }
}
using Mottu.Api.Application.Models;
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Infrastructure.Interfaces;

public interface ISignInManager
{
    Task<Result<string>> CheckPasswordSignIn(ApplicationUser user, string password);
}
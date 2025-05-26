using Mottu.Api.Application.Models;
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Infrastructure.Interfaces;

public interface IUserManager
{
    Task<Result<string>> Create(ApplicationUser user, string password);
    Task<Result<string>> AddToRole(ApplicationUser user, string role);
    Task<Result<ApplicationUser>> FindByName(string username);
    Task<IList<string>?> GetRoles(ApplicationUser user);
}
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Tests.Factories;

public static class ApplicationUserFactory
{
    public static ApplicationUser ApplicationUser(string username)
    {
        return new ApplicationUser(username);
    }
}
using Microsoft.AspNetCore.Identity;

namespace Mottu.Api.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    protected ApplicationUser()
    {

    }

    public ApplicationUser(string username, string email)
    {
        UserName = username;
        Email = email;
    }

    public ApplicationUser(string username) : base(username)
    {

    }
}
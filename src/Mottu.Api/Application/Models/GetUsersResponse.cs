namespace Mottu.Api.Application.Models;

public class GetUsersResponse
{
    public GetUsersResponse(string username, string email, string roles)
    {
        Username = username;
        Email = email;
        Roles = roles;
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string Roles { get; set; }
}
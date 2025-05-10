namespace Mottu.Api.Application.Models;

public class RegisterUserResponse
{
    public RegisterUserResponse(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; set; }
}
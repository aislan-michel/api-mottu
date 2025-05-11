using System.ComponentModel;

namespace Mottu.Api.Application.Models;

public class LoginUserRequest
{
    public string Username { get; set; }

    [PasswordPropertyText]
    public string Password { get; set; }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Application.Models;

public class RegisterUserRequest
{
    [EmailAddress]
    public string Email { get; set; }

    [PasswordPropertyText]
    public string Password { get; set; }

    public string Role { get; set; }
}
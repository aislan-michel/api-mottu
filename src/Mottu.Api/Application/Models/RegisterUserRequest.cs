using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class RegisterUserRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo username é obrigatório")]
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo e-mail é obrigatório")]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo senha é obrigatório")]
    [PasswordPropertyText]
    [JsonPropertyName("senha")]
    public string? Password { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo role é obrigatório")]
    [JsonPropertyName("role")]
    public string? Role { get; set; }
}
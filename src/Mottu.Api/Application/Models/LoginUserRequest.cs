using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class LoginUserRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo username é obrigatório")]
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo senha é obrigatório")]
    [PasswordPropertyText]
    [JsonPropertyName("senha")]
    public string? Password { get; set; }
}
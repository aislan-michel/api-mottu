using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class RegisterDeliveryManRequest
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

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo nome é obrigatório")]
    [JsonPropertyName("nome")]
    public string? Name { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CNPJ é obrigatório")]
    [JsonPropertyName("cnpj")]
    public string? CompanyRegistrationNumber { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo data de nascimento é obrigatório")]
    [JsonPropertyName("data_nascimento")]
    public DateOnly DateOfBirth { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo número da CNH é obrigatório")]
    [JsonPropertyName("numero_cnh")]
    public string? DriverLicense { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo tipo da CNH é obrigatório")]
    [JsonPropertyName("tipo_cnh")]
    public string? DriverLicenseType { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo imagem da CNH é obrigatório")]
    [Base64String]
    [JsonPropertyName("imagem_cnh")]
    public string? DriverLicenseImageBase64 { get; set; }

}
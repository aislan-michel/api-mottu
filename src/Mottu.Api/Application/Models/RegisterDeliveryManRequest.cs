using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RegisterDeliveryManRequest
{
    [JsonPropertyName("nome_usuario")]
    public string Username { get; set; }

    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [PasswordPropertyText]
    [JsonPropertyName("senha")]
    public string Password { get; set; }

    [JsonPropertyName("nome")]
    public string Name { get; set; }

    [JsonPropertyName("cnpj")]
    public string CompanyRegistrationNumber { get; set; }

    [JsonPropertyName("data_nascimento")]
    public DateOnly DateOfBirth { get; set; }

    [JsonPropertyName("numero_cnh")]
    public string DriverLicense { get; set; }

    [JsonPropertyName("tipo_cnh")]
    public string DriverLicenseType { get; set; }

    /// <summary>
    /// Driver license image in base64 string
    /// </summary>
    [JsonPropertyName("imagem_cnh")]
    public string DriverLicenseImage { get; set; }

}
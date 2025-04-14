using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class PostDeliveryManRequest
{
    [JsonPropertyName("nome")]
    //[DefaultValue("João da Silva")]
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
using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class PatchDriverLicenseImageRequest
{
    /// <summary>
    /// Driver license image in base64 string
    /// </summary>
    [JsonPropertyName("imagem_cnh")]
    public string DriverLicenseImage { get; set; }
}
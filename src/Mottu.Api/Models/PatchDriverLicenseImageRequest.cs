using System.Text.Json.Serialization;

namespace Mottu.Api.Models;

public class PatchDriverLicenseImageRequest
{
    /// <summary>
    /// Driver license image in base64 string
    /// </summary>
    [JsonPropertyName("imagem_cnh")]
    public string DriverLicenseImage { get; set; }
}
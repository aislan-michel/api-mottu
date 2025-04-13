using System.Text.Json.Serialization;

namespace Mottu.Api.Models;

public class PatchMotorcycleRequest
{
	[JsonPropertyName("placa")]
	public string? Plate { get; set; }
}

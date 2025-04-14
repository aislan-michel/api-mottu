using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class PatchMotorcycleRequest
{
	[JsonPropertyName("placa")]
	public string? Plate { get; set; }
}

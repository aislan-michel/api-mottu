using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mottu.Api.Models;

public class PostMotorcycleRequest
{
	[JsonPropertyName("ano")]
    public int Year { get; set; }

	[JsonPropertyName("modelo")]
	public string? Model { get; set; }

	[JsonPropertyName("placa")]
	public string? Plate { get; set; }
}
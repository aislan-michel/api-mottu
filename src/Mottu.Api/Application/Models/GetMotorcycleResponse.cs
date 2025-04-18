using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class GetMotorcycleResponse
{
    public GetMotorcycleResponse(string id, int year, string? model, string? plate)
    {
        Id = id;
        Year = year;
        Model = model;
        Plate = plate;
    }

    [JsonPropertyName("identificador")]
    public string Id { get; set; }

    [JsonPropertyName("ano")]
    public int Year { get; set; }

    [JsonPropertyName("modelo")]
	public string? Model { get; set; }

    [JsonPropertyName("placa")]
	public string? Plate { get; set; }
}
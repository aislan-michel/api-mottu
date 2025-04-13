using System.Text.Json.Serialization;

namespace Mottu.Api.Models;

public class GetMotorcycleResponse
{
    public GetMotorcycleResponse(int id, int year, string? model, string? plate)
    {
        Id = id;
        Year = year;
        Model = model;
        Plate = plate;
    }

    [JsonPropertyName("identificador")]
    public int Id { get; set; }

    [JsonPropertyName("ano")]
    public int Year { get; set; }

    [JsonPropertyName("modelo")]
	public string? Model { get; set; }

    [JsonPropertyName("placa")]
	public string? Plate { get; set; }
}
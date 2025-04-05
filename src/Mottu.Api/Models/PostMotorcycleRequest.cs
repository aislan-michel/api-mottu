namespace Mottu.Api.Models;

public class PostMotorcycleRequest
{
    public int Year { get; set; }
	public string? Model { get; set; }
	public string? Plate { get; set; }
}
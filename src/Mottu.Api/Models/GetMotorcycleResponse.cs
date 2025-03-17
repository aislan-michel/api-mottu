namespace Mottu.Api.Models;

public class GetMotorcycleResponse
{
    public GetMotorcycleResponse(int id, int year, string model, string plate)
    {
        Id = id;
        Year = year;
        Model = model;
        Plate = plate;
    }

    public int Id { get; set; }
    public int Year { get; set; }
	public string Model { get; set; }
	public string Plate { get; set; }
}
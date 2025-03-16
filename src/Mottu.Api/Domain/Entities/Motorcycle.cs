namespace Mottu.Api.Domain.Entities;

public class Motorcycle
{
    public Motorcycle(int id, int year, string model, string plate)
    {
        Id = id;
        Year = year;
        Model = model;
        Plate = plate;
    }

    public int Id { get; private set; }
    public int Year { get; private set; }
	public string Model { get; private set; }
	public string Plate { get; private set; }
}

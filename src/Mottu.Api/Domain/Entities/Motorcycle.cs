namespace Mottu.Api.Domain.Entities;

public class Motorcycle
{
    public Motorcycle(int year, string? model, string? plate)
    {
        Id = Guid.NewGuid().ToString();
        Year = year;
        Model = model;
        Plate = plate;
    }

    public string Id { get; private set; }
    public int Year { get; private set; }
	public string? Model { get; private set; }
	public string? Plate { get; private set; }

    public void UpdatePlate(string? plate)
    {
        if(string.IsNullOrWhiteSpace(plate))
        {
            throw new ArgumentNullException(nameof(plate), "Placa não pode ser nulo ou vazio");
        }

        Plate = plate;
    }
}

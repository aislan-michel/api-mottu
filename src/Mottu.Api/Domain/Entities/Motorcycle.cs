namespace Mottu.Api.Domain.Entities;

public class Motorcycle : BaseEntity
{
    //for ef
    protected Motorcycle()
    {

    }

    public Motorcycle(int year, string? model, string? plate)
    {
        Year = year;
        Model = model;
        Plate = plate;
    }

    public int Year { get; private set; }
    public string? Model { get; private set; }
    public string? Plate { get; private set; }

    public string? RentId { get; private set; }
    public Rent? Rent { get; private set; }

    public void UpdatePlate(string? plate)
    {
        Plate = plate;
    }

    public void UpdateRentId(string rentId)
    {
        RentId = rentId;
    }
}

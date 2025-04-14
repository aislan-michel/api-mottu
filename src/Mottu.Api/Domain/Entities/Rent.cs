namespace Mottu.Api.Domain.Entities;

public class Rent
{
    public Rent(
        int id,
        DeliveryMan deliveryMan, Motorcycle motorcycle, 
        DateTime startDate, DateTime endDate, DateTime expectedEndDate, 
        int plan, decimal dailyValue, DateTime? returnDate)
    {
        Id = id;
        DeliveryMan = deliveryMan;
        Motorcycle = motorcycle;
        StartDate = startDate;
        EndDate = endDate;
        ExpectedEndDate = expectedEndDate;
        Plan = plan;
        DailyValue = dailyValue;
        ReturnDate = returnDate;
    }

    public int Id { get; private set; }
    public DeliveryMan DeliveryMan { get; private set; }
    public Motorcycle Motorcycle { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime ExpectedEndDate { get; private set; }
    public int Plan { get; private set; }
    public decimal DailyValue { get; private set; }
    public DateTime? ReturnDate { get; set; }
}

public enum Plan
{}
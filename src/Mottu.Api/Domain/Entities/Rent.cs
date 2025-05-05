namespace Mottu.Api.Domain.Entities;

public class Rent
{
    public Rent(DeliveryMan deliveryMan, Motorcycle motorcycle, Plan plan)
    {
        Id = Guid.NewGuid().ToString();
        DeliveryMan = deliveryMan;
        Motorcycle = motorcycle;
        Plan = plan;
        
        SetDates();
    }

    public string Id { get; private set; }
    public DeliveryMan DeliveryMan { get; private set; }
    public Motorcycle Motorcycle { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime ExpectedEndDate { get; private set; }
    public Plan Plan { get; private set; }
    public DateTime? ReturnDate { get; private set; }
    public decimal? TotalAmountPayable { get; private set; }

    private void SetDates()
    {
        var today = DateTime.Today;
        var startDate = new DateTime(today.Year, today.Month, today.Day + 1, 00, 00, 00);
        var endDate = startDate.AddDays(Plan.Days).AddSeconds(-1);
        var expectedEndDate = endDate;

        StartDate = startDate;
        EndDate = endDate;
        ExpectedEndDate = expectedEndDate;
    }

    public void UpdateReturnDate(DateTime? returnDate)
    {
        ReturnDate = returnDate;
    }

    public void SetTotalAmountPayable()
    {
        if (!ReturnDate.HasValue)
        {
            TotalAmountPayable = Plan.GetTotalDailyRate();
            return;
        }

        var returnDate = ReturnDate.Value.Date;
        var expectedReturnDate = ExpectedEndDate.Date;
        var dailyRate = Plan.DailyRate;
        var plannedDays = Plan.Days;

        if (returnDate < expectedReturnDate)
        {
            int usedDays = (returnDate - StartDate.Date).Days;
            usedDays = usedDays < 1 ? 1 : usedDays;

            var unusedDays = plannedDays - usedDays;
            unusedDays = unusedDays < 0 ? 0 : unusedDays;

            decimal baseValue = dailyRate * usedDays;

            decimal penaltyPercentage = Plan.Days switch
            {
                7 => 0.20m,
                15 => 0.40m,
                30 => 0.60m,
                45 => 0.80m,
                50 => 1.00m,
                _ => 0m
            };

            decimal penalty = dailyRate * unusedDays * penaltyPercentage;

            TotalAmountPayable = baseValue + penalty;
        }
        else if (returnDate > expectedReturnDate)
        {
            int extraDays = (returnDate - expectedReturnDate).Days;
            decimal extraCharge = extraDays * 50.00m;
            TotalAmountPayable = Plan.GetTotalDailyRate() + extraCharge;
        }
        else
        {
            TotalAmountPayable = Plan.GetTotalDailyRate();
        }
    }
}

public class Plan
{
    public Plan(int days)
    {
        Days = days;
        CalculateDailyRate();
    }

    public int Days { get; private set; }
    public decimal DailyRate { get; private set; }

    public decimal GetTotalDailyRate()
    {
        return DailyRate * Days; 
    }

    private void CalculateDailyRate()
    {
        DailyRate = Days switch
        {
            7 => 30,
            15 => 28,
            30 => 22,
            45 => 20,
            50 => 18,
            _ => throw new ArgumentOutOfRangeException(nameof(Days), "plano inválido"),
        };
    }
}
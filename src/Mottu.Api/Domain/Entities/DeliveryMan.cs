namespace Mottu.Api.Domain.Entities;

public class DeliveryMan : BaseEntity
{
    //for rf
    protected DeliveryMan()
    {

    }

    public DeliveryMan(string? name, string? companyRegistrationNumber, DateOnly dateOfBirth,
        DriverLicense driverLicense, string? userId)
    {
        Name = name;
        CompanyRegistrationNumber = companyRegistrationNumber;
        DateOfBirth = dateOfBirth;
        DriverLicense = driverLicense;
        UserId = userId;
    }

    public string? Name { get; private set; }
    public string? CompanyRegistrationNumber { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public DriverLicense DriverLicense { get; private set; } = new DriverLicense(default, default, default);

    public string? UserId { get; private set; }

    public string? RentId { get; set; }
    public Rent? Rent { get; set; }
    
    public void UpdateRentId(string rentId)
    {
        RentId = rentId;
    }
}

public class DriverLicense
{
    protected DriverLicense()
    {
        
    }

    public DriverLicense(string? number, string? type, string? imagePath)
    {
        Number = number;
        Type = type;
        ImagePath = imagePath;
    }

    public string? Number { get; private set; }
    public string? Type { get; private set; }

    /// <summary>
    /// path of image
    /// </summary>
    public string? ImagePath { get; private set; }

    public void UpdateImagePath(string imagePath)
    {
        if(string.IsNullOrWhiteSpace(imagePath))
        {
            throw new ArgumentNullException(nameof(imagePath), "Imagem da CNH não pode ser nulo ou vazio");
        }

        ImagePath = imagePath;
    }

    public bool TypeIsA()
    {
        return Type == "A";
    }
}
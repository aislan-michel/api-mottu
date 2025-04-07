namespace Mottu.Api.Domain.Entities;

public class DeliveryMan
{
    public DeliveryMan(
        int id, string name, string companyRegistrationNumber, DateTime dateOfBirth, 
        DriverLicense driverLicense)
    {
        Id = id;
        Name = name;
        CompanyRegistrationNumber = companyRegistrationNumber;
        DateOfBirth = dateOfBirth;
        DriverLicense = driverLicense;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string CompanyRegistrationNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DriverLicense DriverLicense { get; private set; }
}

public class DriverLicense
{
    public DriverLicense(string number, string type, string image)
    {
        Number = number;
        Type = type;
        Image = image;
    }

    public string Number { get; set; }
    public string Type { get; set; }
    public string Image { get; set; }
}
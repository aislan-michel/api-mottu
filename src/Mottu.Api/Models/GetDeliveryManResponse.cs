namespace Mottu.Api.Models;

public class GetDeliveryManResponse
{
    public GetDeliveryManResponse(
        int id, string name, string companyRegistrationNumber, DateOnly dateOfBirth, 
        GetDriverLicenseResponse driverLicense)
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
    public DateOnly DateOfBirth { get; set; }
    public GetDriverLicenseResponse DriverLicense { get; private set; }
}

public class GetDriverLicenseResponse
{
    public GetDriverLicenseResponse(string number, string type, string image)
    {
        Number = number;
        Type = type;
        Image = image;
    }

    public string Number { get; set; }
    public string Type { get; set; }
    public string Image { get; set; }
}
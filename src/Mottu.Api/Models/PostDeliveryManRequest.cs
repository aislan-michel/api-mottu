namespace Mottu.Api.Models;

public class PostDeliveryManRequest
{
    public string Name { get; set; }
    public string CompanyRegistrationNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string DriverLicense { get; set; }
    public string DriverLicenseType { get; set; }
    /// <summary>
    /// Driver license image in base64 string
    /// </summary>
    public string DriverLicenseImage { get; set; }
}
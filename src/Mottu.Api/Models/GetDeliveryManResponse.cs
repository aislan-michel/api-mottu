using System.Text.Json.Serialization;

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

    [JsonPropertyName("identificador")]
    public int Id { get; set; }

    [JsonPropertyName("nome")]
    public string Name { get; set; }

    [JsonPropertyName("cnpj")]
    public string CompanyRegistrationNumber { get; set; }

    [JsonPropertyName("data_nascimento")]
    public DateOnly DateOfBirth { get; set; }

    [JsonPropertyName("cnh")]
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

    [JsonPropertyName("numero")]
    public string Number { get; set; }

    [JsonPropertyName("tipo")]
    public string Type { get; set; }

    [JsonPropertyName("imagem")]
    public string Image { get; set; }
}
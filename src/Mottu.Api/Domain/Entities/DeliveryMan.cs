namespace Mottu.Api.Domain.Entities;

public class DeliveryMan
{
    public DeliveryMan(
        int id, string name, string companyRegistrationNumber, DateOnly dateOfBirth, 
        DriverLicense driverLicense)
    {
        Id = id;
        Name = name;
        CompanyRegistrationNumber = companyRegistrationNumber;
        DateOfBirth = dateOfBirth;
        DriverLicense = driverLicense;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string CompanyRegistrationNumber { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
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

    public string Number { get; private set; }
    public string Type { get; private set; }

    /// <summary>
    /// path of image
    /// </summary>
    public string Image { get; private set; }

    public void UpdateImage(string image)
    {
        if(string.IsNullOrWhiteSpace(image))
        {
            throw new ArgumentNullException(nameof(image), "Imagem da CNH não pode ser nulo ou vazio");
        }

        Image = image;
    }

    public bool TypeIsA()
    {
        return Type == "A";
    }
}
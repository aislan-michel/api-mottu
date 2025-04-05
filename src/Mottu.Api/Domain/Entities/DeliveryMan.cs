namespace Mottu.Api.Domain.Entities;

public class DeliveryMan
{
    public DeliveryMan(
        int id, string name, string cnpj, DateTime dateOfBirth, 
        string cnh, string cnhType, string cnhImage)
    {
        Id = id;
        Name = name;
        Cnpj = cnpj;
        DateOfBirth = dateOfBirth;
        Cnh = cnh;
        CnhType = cnhType;
        CnhImage = cnhImage;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Cnh { get; set; }
    public string CnhType { get; set; }
    public string CnhImage { get; set; }
}
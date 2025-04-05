namespace Mottu.Api.Models;

public class PostDeliveryManRequest
{
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Cnh { get; set; }
    public string CnhType { get; set; }
    public string CnhImage { get; set; }
}
using Microsoft.AspNetCore.Mvc;

namespace Mottu.Api.Models;

public class GetMotorcyclesRequest
{
    [FromQuery(Name = "placa")]
    public string? Plate { get; set; }
}
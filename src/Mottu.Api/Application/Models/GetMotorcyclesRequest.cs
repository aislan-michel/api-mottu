using Microsoft.AspNetCore.Mvc;

namespace Mottu.Api.Application.Models;

public class GetMotorcyclesRequest
{
    [FromQuery(Name = "placa")]
    public string? Plate { get; set; }
}
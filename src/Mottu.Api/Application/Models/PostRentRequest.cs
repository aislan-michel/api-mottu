using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class PostRentRequest
{
    [JsonPropertyName("moto_id")] 
    public string MotorcycleId { get; set; }
/*
    [JsonPropertyName("data_inicio")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("data_termino")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("data_previsao_termino")]
    public DateTime ExpectedEndDate { get; set; }*/

    [JsonPropertyName("plano")]
    public int Plan { get; set; }
}
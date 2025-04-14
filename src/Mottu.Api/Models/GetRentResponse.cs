using System.Text.Json.Serialization;

namespace Mottu.Api.Models;

public class GetRentResponse
{
    public GetRentResponse(int id, decimal dailyValue, 
        int deliveryManId, int motorcycleId, 
        DateTime startDate, DateTime endDate, DateTime expectedEndDate, DateTime? returnDate)
    {
        Id = id;
        DailyValue = dailyValue;
        DeliveryManId = deliveryManId;
        MotorcycleId = motorcycleId;
        StartDate = startDate;
        EndDate = endDate;
        ExpectedEndDate = expectedEndDate;
        ReturnDate = returnDate;
    }

    [JsonPropertyName("identificador")]
    public int Id { get; set; }

    [JsonPropertyName("valor_diaria")]
    public decimal DailyValue { get; set; }

    [JsonPropertyName("entregador_id")]
    public int DeliveryManId { get; set; }

    [JsonPropertyName("moto_id")]
    public int MotorcycleId { get; set; }
    
    [JsonPropertyName("data_inicio")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("data_termino")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("data_previsao_termino")]
    public DateTime ExpectedEndDate { get; set; }

    [JsonPropertyName("data_devolucao")]
    public DateTime? ReturnDate { get; set; }
}
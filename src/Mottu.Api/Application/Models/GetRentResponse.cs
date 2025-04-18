using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class GetRentResponse
{
    public GetRentResponse(string id, decimal dailyValue, 
        string deliveryManId, string motorcycleId, 
        DateTime startDate, DateTime endDate, DateTime expectedEndDate, DateTime? returnDate,
        decimal? totalAmountPayable)
    {
        Id = id;
        DailyValue = dailyValue;
        DeliveryManId = deliveryManId;
        MotorcycleId = motorcycleId;
        StartDate = startDate;
        EndDate = endDate;
        ExpectedEndDate = expectedEndDate;
        ReturnDate = returnDate;
        TotalAmountPayable = totalAmountPayable;
    }

    [JsonPropertyName("identificador")]
    public string Id { get; set; }

    [JsonPropertyName("valor_diaria")]
    public decimal DailyValue { get; set; }

    [JsonPropertyName("entregador_id")]
    public string DeliveryManId { get; set; }

    [JsonPropertyName("moto_id")]
    public string MotorcycleId { get; set; }
    
    [JsonPropertyName("data_inicio")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("data_termino")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("data_previsao_termino")]
    public DateTime ExpectedEndDate { get; set; }

    [JsonPropertyName("data_devolucao")]
    public DateTime? ReturnDate { get; set; }

    [JsonPropertyName("total_pagar")]
    public decimal? TotalAmountPayable { get; set; }
}
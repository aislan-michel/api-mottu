using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class PatchRentRequest
{
    [JsonPropertyName("data_devolucao")]
    public DateTime? ReturnDate { get; set; }
}
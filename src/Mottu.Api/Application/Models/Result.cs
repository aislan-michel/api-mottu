using System.Text.Json.Serialization;

namespace Mottu.Api.Application.Models;

public class Result<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; private set; }

    [JsonPropertyName("data")]
    public T? Data { get; private set; }

    [JsonPropertyName("messages")]
    public IEnumerable<string> Messages { get; private set; }

    private Result(bool success, T? data, IEnumerable<string> messages)
    {
        Success = success;
        Data = data;
        Messages = messages;
    }

    public static Result<T> Ok(T? data)
    {
        return new Result<T>(true, data, []);
    }

    public static Result<T> Fail(IEnumerable<string> errors)
    {
        return new Result<T>(false, default, errors);
    }

    public static Result<T> Fail(string error)
    {
        return new Result<T>(false, default, [error]);
    }

    public string GetMessages(string separator = ", ")
    {
        return string.Join(separator, Messages);
    }
}
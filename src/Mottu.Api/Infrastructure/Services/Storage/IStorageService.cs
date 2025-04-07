namespace Mottu.Api.Infrastructure.Services.Storage;

public interface IStorageService
{
    string SaveBase64Image(string base64Image);
}
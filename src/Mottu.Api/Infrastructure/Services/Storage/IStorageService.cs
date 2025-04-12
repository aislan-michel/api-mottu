namespace Mottu.Api.Infrastructure.Services.Storage;

public interface IStorageService
{
    string SaveBase64Image(string imageBase64, string? imageName = null);
    void DeleteImage(string path);
}
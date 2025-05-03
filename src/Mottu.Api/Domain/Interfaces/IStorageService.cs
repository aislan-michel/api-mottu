namespace Mottu.Api.Domain.Interfaces;

public interface IStorageService
{
    string SaveBase64Image(string imageBase64, string? imageName = null);
    void DeleteImage(string path);
}
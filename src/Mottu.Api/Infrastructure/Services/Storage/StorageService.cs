namespace Mottu.Api.Infrastructure.Services.Storage;

public class StorageService : IStorageService
{
    public string SaveBase64Image(string imageBase64, string? imageName = null)
    {
        if (imageBase64.Contains("data:image"))
        {
            var indexOfBase64 = imageBase64.IndexOf("base64,") + "base64,".Length;
            imageBase64 = imageBase64.Substring(indexOfBase64);
        }

        var imageBytes = Convert.FromBase64String(imageBase64);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "images");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        if(string.IsNullOrWhiteSpace(imageName))
        {
            imageName = Guid.NewGuid().ToString();
        }

        var pathImage = Path.Combine(path, imageName);

        File.WriteAllBytes(pathImage, imageBytes);

        return pathImage;
    }

    public void DeleteImage(string path)
    {
        File.Delete(path);
    }
}
namespace Mottu.Api.Infrastructure.Services.Storage;

public class StorageService : IStorageService
{
    public string SaveBase64Image(string base64Image)
    {
        if (base64Image.Contains("data:image"))
        {
            var indexOfBase64 = base64Image.IndexOf("base64,") + "base64,".Length;
            base64Image = base64Image.Substring(indexOfBase64);
        }

        var imageBytes = Convert.FromBase64String(base64Image);

        var path = Path.Combine(Directory.GetCurrentDirectory(), "images");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var imageName = $"{Guid.NewGuid()}.jpg"; 
        var pathImage = Path.Combine(path, imageName);

        File.WriteAllBytes(pathImage, imageBytes);

        return pathImage;
    }
}
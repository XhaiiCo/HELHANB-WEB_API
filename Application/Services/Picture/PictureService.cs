using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace API.Utils.Picture;

public class PictureService : IPictureService
{
    public static IWebHostEnvironment _environment;

    public static readonly string[] AllowedFileTypes = { "image/jpeg", "image/png", "image/webp" };

    //'/' for jpeg, 'i' for png, 'U' for webp
    public static readonly string AllowedBase64Extensions = "/iU";

    public PictureService(
        IWebHostEnvironment environment
    )
    {
        _environment = environment;
    }

    public bool ValidPictureType(string contentType)
    {
        return AllowedFileTypes.Contains(contentType);
    }

    public bool ValidExtensions(IEnumerable<string> picturesBase64)
    {
        foreach (var pic in picturesBase64)
        {
            if (!AllowedBase64Extensions.Contains(pic[0])) ;
            {
                return false;
            }
        }

        return true;
    }

    public string GenerateUniqueFileName(int userId)
    {
        return userId + "_" + DateTime.Now.Ticks;
    }

    public string GetExtension(string base64)
    {
        char firstChar = base64[0];

        if (firstChar == '/')
        {
            return ".jpeg";
        }
        else if (firstChar == 'i')
        {
            return ".png";
        }

        return ".webp";
    }

    public void CreateDirectory(string path)
    {
        if (!Directory.Exists(_environment.WebRootPath + path))
        {
            Directory.CreateDirectory(_environment.WebRootPath + path);
        }
    }

    public void RemoveFile(string path)
    {
        if (path == "\\Upload\\ProfilePicture\\default_user_pic.png") return;

        if (File.Exists(_environment.WebRootPath + path))
        {
            File.Delete(_environment.WebRootPath + path);
        }
    }

    public void UploadPicture(string path, string fileName, IFormFile picture)
    {
        this.CreateDirectory(path);

        using var fileStream = System.IO.File.Create(_environment.WebRootPath +
                                                     path +
                                                     fileName);
        //Copy the file to the directory
        picture.CopyTo(fileStream);
        fileStream.Flush();
    }

    public void UploadBase64Picture(string basepath, string filePath, string base64Picture)
    {
        this.CreateDirectory(basepath);

        var bytes = Base64ToBytes(base64Picture);

        File.WriteAllBytes(_environment.WebRootPath + filePath, bytes);
    }

    public byte[] Base64ToBytes(string base64Picture)
    {
        return Convert.FromBase64String(base64Picture);
    }

    public byte[] PathToBytes(string path)
    {
        return System.IO.File.ReadAllBytes(_environment.WebRootPath + path);
    }
    public bool ContainsImage(IEnumerable<byte[]> images, byte[] image)
    {
        return images.Any(image.SequenceEqual);
    }
}
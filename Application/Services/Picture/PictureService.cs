using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace API.Utils.Picture;

public class PictureService : IPictureService
{
    private static IWebHostEnvironment _environment;

    private static readonly string[] AllowedFileTypes = { "image/jpeg", "image/png", "image/webp" };

    //'/' for jpeg, 'i' for png, 'U' for webp
    private static readonly string AllowedBase64Extensions = "/iU";

    public PictureService(
        IWebHostEnvironment environment
    )
    {
        _environment = environment;
    }

    /// <summary>
    /// If the contentType is in the AllowedFileTypes list, return true
    /// </summary>
    /// <param name="contentType">The content type of the file.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public bool ValidPictureType(string contentType)
    {
        return AllowedFileTypes.Contains(contentType);
    }

    /// <summary>
    /// It takes a list of base64 strings and returns true if all of them have a valid extension
    /// </summary>
    /// <param name="picturesBase64">A list of base64 strings that represent the pictures.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
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

    /// <summary>
    /// It takes a userId and returns a string that is the userId concatenated with the current time in ticks
    /// </summary>
    /// <param name="userId">The user id of the user who is uploading the file.</param>
    /// <returns>
    /// A string that is the userId concatenated with the current time in ticks.
    /// </returns>
    public string GenerateUniqueFileName(int userId)
    {
        return userId + "_" + DateTime.Now.Ticks;
    }

    public string GetExtensionOfBase64(string base64)
    {
        var firstChar = base64[0];

        return firstChar switch
        {
            '/' => ".jpeg",
            'i' => ".png",
            _ => ".webp"
        };
    }

    /// <summary>
    /// If the directory doesn't exist, create it
    /// </summary>
    /// <param name="path">The path to the directory you want to create.</param>
    public void CreateDirectory(string path)
    {
        if (!Directory.Exists(_environment.WebRootPath + path))
        {
            Directory.CreateDirectory(_environment.WebRootPath + path);
        }
    }

    /// <summary>
    /// If the file exists, delete it
    /// </summary>
    /// <param name="path">The path of the file to be deleted.</param>
    public void RemoveFile(string path)
    {
        if (path == "\\Upload\\ProfilePicture\\default_user_pic.png") return;

        if (File.Exists(_environment.WebRootPath + path))
        {
            File.Delete(_environment.WebRootPath + path);
        }
    }

    /// <summary>
    /// It creates a directory if it doesn't exist, then creates a file stream and copies the file to the directory
    /// </summary>
    /// <param name="path">The path to the directory where the file will be saved.</param>
    /// <param name="fileName">The name of the file you want to save it as.</param>
    /// <param name="picture">This is the file that is being uploaded.</param>
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

    /// <summary>
    /// It takes a base64 string, converts it to bytes, and then writes those bytes to a file
    /// </summary>
    /// <param name="basepath">The path to the directory where the file will be saved.</param>
    /// <param name="filePath">The path to the file you want to save.</param>
    /// <param name="base64Picture">The base64 string of the picture</param>
    public void UploadBase64Picture(string basepath, string filePath, string base64Picture)
    {
        this.CreateDirectory(basepath);

        var bytes = Base64ToBytes(base64Picture);

        File.WriteAllBytes(_environment.WebRootPath + filePath, bytes);
    }

    /// <summary>
    /// It takes a string of base64 characters and converts it to a byte array
    /// </summary>
    /// <param name="base64Picture">The base64 string that you want to convert to a byte array.</param>
    /// <returns>
    /// A byte array.
    /// </returns>
    public byte[] Base64ToBytes(string base64Picture)
    {
        return Convert.FromBase64String(base64Picture);
    }

    public byte[] PathToBytes(string path)
    {
        return System.IO.File.ReadAllBytes(_environment.WebRootPath + path);
    }

    /// <summary>
    /// It takes a list of byte arrays and a byte array and returns true if the list contains the byte array
    /// </summary>
    /// <param name="images">The collection of images to search through.</param>
    /// <param name="image">The image you want to check if it's in the list of images.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public bool ContainsImage(IEnumerable<byte[]> images, byte[] image)
    {
        return images.Any(image.SequenceEqual);
    }
}
namespace API.Utils.Picture;

public class PictureService : IPictureService
{
    public static IWebHostEnvironment _environment;
    public static readonly string[] AllowedFileTypes = { "image/jpeg", "image/png" };

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

    public string GenerateUniqueFileName(int userId, string fileName)
    {
        return userId + "_" + DateTime.Now.Ticks + "_" + fileName.Replace(" ", "");
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
}
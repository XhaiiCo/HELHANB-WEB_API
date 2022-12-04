using Microsoft.AspNetCore.Http;

namespace API.Utils.Picture;

public interface IPictureService
{
    public bool ValidPictureType(string contentType);
    public string GenerateUniqueFileName(int userId);

    public void CreateDirectory(string path);

    public void RemoveFile(string path);

    public void UploadPicture(string path, string fileName, IFormFile picture);
    
    public bool ValidExtensions(IEnumerable<string> picturesBase64);

    public string GetExtension(string base64);

    public void UploadBase64Picture(string basepath, string fullpath, string base64Picture);
}
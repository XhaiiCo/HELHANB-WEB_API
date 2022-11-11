namespace API.Utils.Picture;

public interface IPictureService
{
    public bool ValidPictureType(string contentType);
    public string GenerateUniqueFileName(int userId, string fileName);

    public void CreateDirectory(string path);

    public void UploadPicture(string path, string fileName, IFormFile picture);
}
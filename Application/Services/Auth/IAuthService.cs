namespace Application.Services.Auth;

public interface IAuthService
{
    public string GenerateRandomSalt();
    public string HashPassword(string password);
    public bool ValidatePassword(string password, string correctHash);
}
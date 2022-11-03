namespace Application.Services.Auth;
using BCrypt.Net;

public class AuthService: IAuthService
{
    public string GenerateRandomSalt()
    {
        return BCrypt.GenerateSalt(12);
    }

    public string HashPassword(string password)
    {
        return BCrypt.HashPassword(password, GenerateRandomSalt());
    }

    public bool ValidatePassword(string password, string correctHash)
    {
        return BCrypt.Verify(password, correctHash);
    }
}
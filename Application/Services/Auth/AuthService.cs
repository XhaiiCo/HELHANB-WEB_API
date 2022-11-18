using Azure;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Auth;
using BCrypt.Net;

public class AuthService: IAuthService
{
    /// <summary>
    /// It generates a random salt using the BCrypt library
    /// </summary>
    /// <returns>
    /// A random salt.
    /// </returns>
    public string GenerateRandomSalt()
    {
        return BCrypt.GenerateSalt(12);
    }

    /// <summary>
    /// It hashes the password using the BCrypt algorithm.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>
    /// A string that is the hashed password.
    /// </returns>
    public string HashPassword(string password)
    {
        return BCrypt.HashPassword(password, GenerateRandomSalt());
    }

    /// <summary>
    /// If the password is correct, the function returns true. If the password is incorrect, the function returns false
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="correctHash">The hash from the database.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public bool ValidatePassword(string password, string correctHash)
    {
        return BCrypt.Verify(password, correctHash);
    }
}
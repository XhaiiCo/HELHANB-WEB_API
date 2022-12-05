using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.UseCases.Users.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Token;

public class TokenService : ITokenService
{
    private const double EXPIRY_DURATION_MINUTES = 180;

    /// <summary>
    /// Returns a JWT token
    /// </summary>
    /// <param name="key">This is the secret key that will be used to sign the token.</param>
    /// <param name="issuer">The issuer of the token.</param>
    /// <param name="DtoTokenUser">This is a DTO that contains the user's id and role name.</param>
    /// <returns>
    /// A JWT token
    /// </returns>
    public string BuildToken(string key, string issuer, DtoTokenUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.RoleName)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    /// <summary>
    /// Returns true if the token is valid, and false if it is not
    /// </summary>
    /// <param name="key">The secret key used to sign the token.</param>
    /// <param name="issuer">The issuer of the token.</param>
    /// <param name="token">The token to validate</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public bool IsTokenValid(string key, string issuer, string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(key);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
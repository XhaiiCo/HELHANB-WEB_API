using Application.UseCases.Users.Dtos;

namespace Application.Services.Token;

public interface ITokenService
{
   string BuildToken(string key, string issuer, DtoTokenUser user);
   bool IsTokenValid(string key, string issuer, string token);
}
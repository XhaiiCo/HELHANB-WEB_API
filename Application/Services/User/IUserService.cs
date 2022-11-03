namespace Application.Services.User;

public interface IUserService
{
    Domain.User FetchByEmail(string email);
}
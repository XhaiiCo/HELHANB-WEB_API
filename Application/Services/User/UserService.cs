using Application;
using Application.Services.User;
using Domain;
using Infrastructure.Ef;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Domain.User FetchByEmail(string email)
    {
        var dbUser = _userRepository.FetchByEmail(email);
        return Mapper.GetInstance().Map<Domain.User>(dbUser);
    }

    public User FetchById(int id)
    {
        var dbUser = _userRepository.fetchById(id);
        return Mapper.GetInstance().Map<Domain.User>(dbUser);
    }
}
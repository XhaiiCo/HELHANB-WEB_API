namespace Application.Services.User;

public class UserService: IUserService
{
    private readonly IUserService _userService;

    public UserService(IUserService userService)
    {
        _userService = userService;
    }

    public Domain.User FetchByEmail(string email)
    {
        var dbUser = _userService.FetchByEmail(email);
        return Mapper.GetInstance().Map<Domain.User>(dbUser);
    }
}
using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;

namespace Application.UseCases.Users;

public class UseCaseCountUsers: IUseCaseParameterizedQuery<int, DtoInputFilteringUsers>
{
    private readonly IUserService _userService;

    public UseCaseCountUsers(IUserService userService)
    {
        _userService = userService;
    }
    
    public int Execute(DtoInputFilteringUsers dto)
    {
        return _userService.FetchAll(dto).Count();
    }
}
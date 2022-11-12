using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;

namespace Application.UseCases.Users;

public class UseCaseFetchAllUsers: IUseCaseQuery<IEnumerable<DtoOutputUser>>
{

    private readonly IUserService _userService;

    public UseCaseFetchAllUsers(IUserService userService)
    {
        _userService = userService;
    }

    public IEnumerable<DtoOutputUser> Execute()
    {
        var users = _userService.FetchAll();
        
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputUser>>(users);
    }
}
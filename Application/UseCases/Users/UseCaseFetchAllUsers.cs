using Application.Services.User;
using Application.UseCases.Roles;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.Repository.User;

namespace Application.UseCases.Users;

public class UseCaseFetchAllUsers : IUseCaseParameterizedQuery<IEnumerable<DtoOutputUser>, DtoInputFilteringUsers>
{

    private readonly IUserService _userService;

    public UseCaseFetchAllUsers(IUserService userService)
    {
        _userService = userService;
    }

    public IEnumerable<DtoOutputUser> Execute(DtoInputFilteringUsers? param)
    {
        var users = _userService.FetchAll(param);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputUser>>(users);
    }
}
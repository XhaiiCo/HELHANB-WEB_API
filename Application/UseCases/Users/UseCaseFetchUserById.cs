using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;

namespace Application.UseCases.Users;

public class UseCaseFetchUserById: IUseCaseParameterizedQuery<DtoOutputUser, int>
{
    private readonly IUserService _userService;

    public UseCaseFetchUserById(IUserService userService)
    {
        _userService = userService;
    }

    public DtoOutputUser Execute(int id)
    {
        var userDb = _userService.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputUser>(userDb);
    }
}
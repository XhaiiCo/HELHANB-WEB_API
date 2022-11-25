using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;

namespace Application.UseCases.Users;

public class UseCaseChangeRoleToHostUser : IUseCaseWriter<DtoOutputUser, int>
{
    private readonly IUserService _userService;

    public UseCaseChangeRoleToHostUser(IUserService userService)
    {
        _userService = userService;
    }

    public DtoOutputUser Execute(int input)
    {
        return Mapper.GetInstance().Map<DtoOutputUser>(_userService.ChangeRole(input, 2));
    }
}
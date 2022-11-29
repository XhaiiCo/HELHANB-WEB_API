using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;

namespace Application.UseCases.Users;

public class UseCaseChangeRole : IUseCaseWriter<DbUser, DtoUserNewRole>
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public UseCaseChangeRole(IUserService userService,
        IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }
    
    public DbUser Execute(DtoUserNewRole userNewRole)
    {
        return _userRepository.Update(Mapper.GetInstance().Map<DbUser>(_userService.ChangeRole(userNewRole.Id, userNewRole.RoleId)));
    }
}
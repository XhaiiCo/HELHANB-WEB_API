using Application.Services.Role;
using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Microsoft.IdentityModel.Tokens;

namespace Application.UseCases.Users;

public class UseCaseChangeRole : IUseCaseWriter<DtoOutputUser, DtoUserNewRole>
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;
    private readonly IAdRepository _adRepository;

    public UseCaseChangeRole(IUserService userService,
        IUserRepository userRepository,
        IRoleService roleService,
        IAdRepository adRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
        _roleService = roleService;
        _adRepository = adRepository;
    }

    public DtoOutputUser Execute(DtoUserNewRole userNewRole)
    {
        var user = _userService.FetchById(userNewRole.Id);

        // if user has host role and switch to user role
        if (user.Role.Name == "hote")
            // if he has rentings
            if (_adRepository.FetchByUserId(user.Id).Count() != 0)
                throw new Exception();

        if (userNewRole.RoleId == user.Role.Id) throw new Exception();

        var result = _userRepository.Update(Mapper.GetInstance()
            .Map<DbUser>(_userService.ChangeRole(userNewRole.Id, userNewRole.RoleId)));

        return Mapper.GetInstance().Map<DtoOutputUser>(_userService.MapToUser(result));
    }
}
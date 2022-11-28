using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository;

namespace Application.UseCases.Users;

public class UseCaseChangeRole : IUseCaseWriter<DtoOutputUser, DtoUserNewRole>
{
    private readonly IUserService _userService;
    private readonly IRoleRepository _roleRepository;

    public UseCaseChangeRole(IUserService userService, IRoleRepository roleRepository)
    {
        _userService = userService;
        _roleRepository = roleRepository;
    }
    
    public DtoOutputUser Execute(DtoUserNewRole userNewRole)
    {
        return Mapper.GetInstance().Map<DtoOutputUser>(_userService.ChangeRole(userNewRole.Id, _roleRepository.FetchByName(userNewRole.RoleName).Id));
    }
}
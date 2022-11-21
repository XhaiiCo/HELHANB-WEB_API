using Application.Services.User;
using Application.UseCases.Roles;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.Repository.User;

namespace Application.UseCases.Users;

public class UseCaseFetchAllUsers : IUseCaseParameterizedQuery<IEnumerable<DtoOutputUser>, DtoInputFilteringUsers>
{
    private readonly IUserRepository _userRepository;
    private readonly UseCaseFetchAllRoles _useCaseFetchAllRoles;

    public UseCaseFetchAllUsers(IUserRepository userRepository, UseCaseFetchAllRoles useCaseFetchAllRoles)
    {
        _userRepository = userRepository;
        _useCaseFetchAllRoles = useCaseFetchAllRoles;
    }

    public IEnumerable<DtoOutputUser> Execute(DtoInputFilteringUsers param)
    {
        var roles = _useCaseFetchAllRoles.Execute();
        
        var roleId = roles.FirstOrDefault(role => role.Name == param.Role)?.Id;

        var filteringUser = new FilteringUser
        {
            RoleId =  roleId,
            Search = param.Search 
        };

        var users = _userRepository.FetchAll(filteringUser);

        return Mapper.GetInstance().Map<IEnumerable<DtoOutputUser>>(users);
    }
}
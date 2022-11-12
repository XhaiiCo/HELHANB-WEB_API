using Application.UseCases.Roles.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository;

namespace Application.UseCases.Roles;

public class UseCaseFetchAllRoles: IUseCaseQuery<IEnumerable<DtoOutputRole>>
{
    private readonly IRoleRepository _roleRepository;

    public UseCaseFetchAllRoles(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public IEnumerable<DtoOutputRole> Execute()
    {
        var dbRoles =  _roleRepository.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputRole>>(dbRoles);
    }
}
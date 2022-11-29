using Application.UseCases.Utils;
using Infrastructure.Ef.Repository;

namespace Application.UseCases.Roles;

public class UseCaseFetchRoleNameById : IUseCaseWriter<string, int>
{
    private readonly IRoleRepository _roleRepository;

    public UseCaseFetchRoleNameById(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public string Execute(int id)
    {
        return _roleRepository.FetchById(id).Name;
    }
}
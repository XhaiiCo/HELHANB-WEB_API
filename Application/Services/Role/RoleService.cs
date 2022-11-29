using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository;

namespace Application.Services.Role;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    /// <summary>
    /// Fetch a role from the database and map it to a Role object
    /// </summary>
    /// <param name="id">The id of the role to fetch</param>
    /// <returns>
    /// A Role object
    /// </returns>
    public Domain.Role FetchById(int id)
    {
        var dbRole = _roleRepository.FetchById(id);

        return MapToRole(dbRole);
    }

    /// <summary>
    /// Map the DbRole to a Role
    /// </summary>
    /// <param name="dbRole">The database entity that we are mapping from.</param>
    /// <returns>
    /// A Role object
    /// </returns>
    public Domain.Role MapToRole(DbRole dbRole)
    {
        return Mapper.GetInstance().Map<Domain.Role>(dbRole);;
    }
}
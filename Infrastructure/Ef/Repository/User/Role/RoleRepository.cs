using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository;

public class RoleRepository: IRoleRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public RoleRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public IEnumerable<DbRole> FetchAll()
    {
        using var context = _contextProvider.NewContext();

        return context.Roles.ToList();
    }

    public DbRole FetchById(int id)
    {
        using var context = _contextProvider.NewContext();

        var role = context.Roles.FirstOrDefault(role => role.Id == id);

        if (role == null)
            throw new KeyNotFoundException($"Le role avec l'id: {id} n'a pas été trouvé");
        
        return role ;
    }

    public DbRole FetchByName(string roleName)
    {
        using var context = _contextProvider.NewContext();
        
        var role = context.Roles.FirstOrDefault(role => role.Name == roleName);

        if (role == null)
            throw new KeyNotFoundException($"Le role {roleName} n'a pas été trouvé");
        
        return role;
    }
}
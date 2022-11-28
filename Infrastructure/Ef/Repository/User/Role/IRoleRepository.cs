using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository;

public interface IRoleRepository
{
    public IEnumerable<DbRole> FetchAll();
    public DbRole FetchById(int id);

    public DbRole FetchByName(string roleName);
}
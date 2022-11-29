using Infrastructure.Ef.DbEntities;

namespace Application.Services.Role;

public interface IRoleService
{
    Domain.Role FetchById(int id);
    public Domain.Role MapToRole(DbRole dbRole);
}
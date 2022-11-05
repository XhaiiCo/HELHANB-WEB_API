using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef;

public interface IUserRepository
{
    IEnumerable<DbUser> FetchAll();
    DbUser Create(DbUser user) ;

    DbUser FetchByEmail(string email);
    DbUser FetchById(int id);
    DbUser Update(DbUser user);
}
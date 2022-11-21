using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.User;

namespace Infrastructure.Ef;

public interface IUserRepository
{
    IEnumerable<DbUser> FetchAll(FilteringUser filteringUser);
    DbUser Create(DbUser user) ;

    DbUser FetchByEmail(string email);
    DbUser FetchById(int id);
    DbUser Update(DbUser user);

    DbUser Delete(DbUser user);
}
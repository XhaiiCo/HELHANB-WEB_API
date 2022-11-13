using Infrastructure.Ef.DbEntities;

namespace Application.Services.User;

public interface IUserService
{
    Domain.User FetchByEmail(string email);
    Domain.User FetchById(int id);

    IEnumerable<Domain.User> FetchAll();
    
   Domain.User MapToUser(DbUser dbUser) ;
}
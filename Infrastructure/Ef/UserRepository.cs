using Infrastructure.Utils;

namespace Infrastructure.Ef.DbEntities;

public class UserRepository: IUserRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public UserRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public IEnumerable<DbUser> FetchAll()
    {
        using var context = _contextProvider.NewContext() ;
        return context.Users.ToList() ;
    }

    public DbUser Create(DbUser user)
    {
        using var context = _contextProvider.NewContext();
        context.Users.Add(user);
        context.SaveChanges();
        return user;
    }
}
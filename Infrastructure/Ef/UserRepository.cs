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

    public DbUser FetchByEmail(string email)
    {
        using var context = _contextProvider.NewContext();
        var user = context.Users.FirstOrDefault(user => user.Email == email);

        if (user == null) throw new KeyNotFoundException($"User with email {email} has not been found");

        return user;
    }
}
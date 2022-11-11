using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

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

        if (user == null) throw new KeyNotFoundException($"L'adresse email n'a pas été trouvée");

        return user;
    }

    public DbUser FetchById(int id)
    {
        using var context = _contextProvider.NewContext();
        var user = context.Users.FirstOrDefault(user => user.Id == id);

        if (user == null)
            throw new KeyNotFoundException($"User with id {id} has not been found");

        return user;
    }

    public DbUser Update(DbUser user)
    {
        using var context = _contextProvider.NewContext();
        
        context.Attach(user);
        context.Entry(user).State = EntityState.Modified;
        context.SaveChanges();
        
        return user;
    }
}
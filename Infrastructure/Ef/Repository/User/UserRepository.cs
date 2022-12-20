using Infrastructure.Ef.Repository.User;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Ef.DbEntities;

public class UserRepository : IUserRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public UserRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public IEnumerable<DbUser> FetchAll(FilteringUser? filteringUser)
    {
        using var context = _contextProvider.NewContext();
        
        if (filteringUser == null) return context.Users.ToList();
        
        var dbUsers = context.Users.Where(user => (filteringUser.RoleId == null || user.RoleId == filteringUser.RoleId) && 
                                           (filteringUser.Search == null || 
                                            (user.FirstName.ToLower().Contains(filteringUser.Search.ToLower().Trim()) ||
                                             user.LastName.ToLower().Contains(filteringUser.Search.ToLower().Trim()) ||
                                             user.Email.ToLower().Contains(filteringUser.Search.ToLower().Trim())))).ToList();
        
        if (filteringUser.Offset != null && filteringUser.Limit != null)
        {
            return dbUsers.Skip(Convert.ToInt32(filteringUser.Offset)).Take(Convert.ToInt32(filteringUser.Limit));
        }

        return dbUsers;
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

    public DbUser Delete(DbUser user)
    {
        using var context = _contextProvider.NewContext();

        context.Users.Remove(user);
        context.SaveChanges();

        return user;
    }
}
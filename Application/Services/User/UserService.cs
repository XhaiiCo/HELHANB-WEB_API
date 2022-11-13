using Application;
using Application.Services.User;
using Domain;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public Domain.User FetchByEmail(string email)
    {
        var dbUser = _userRepository.FetchByEmail(email);

        return MapToUser(dbUser);
    }

    public User FetchById(int id)
    {
        var dbUser = _userRepository.FetchById(id);

        return MapToUser(dbUser);
    }

    public IEnumerable<User> FetchAll()
    {
        var dbUsers = _userRepository.FetchAll();
        var users = dbUsers.Select(MapToUser);

        return users;
    }

    public User MapToUser(DbUser dbUser)
    {
        var user = Mapper.GetInstance().Map<User>(dbUser);

        user.Role = new Role
        {
            Id = dbUser.RoleId,
            Name = _roleRepository.FetchById(dbUser.RoleId).Name
        };

        return user;
    }
}
using Application;
using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Domain;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository;
using Infrastructure.Ef.Repository.User;

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

    /// <summary>
    /// Fetch a user by email.
    /// </summary>
    /// <param name="email">The email address of the user to fetch.</param>
    /// <returns>
    /// A Domain.User object
    /// </returns>
    public Domain.User FetchByEmail(string email)
    {
        var dbUser = _userRepository.FetchByEmail(email);

        return MapToUser(dbUser);
    }

    /// <summary>
    /// Fetch a user from the database and map it to a User object
    /// </summary>
    /// <param name="id">The id of the user to fetch</param>
    /// <returns>
    /// A User object
    /// </returns>
    public User FetchById(int id)
    {
        var dbUser = _userRepository.FetchById(id);

        return MapToUser(dbUser);
    }

    /// <summary>
    /// Fetch all users from the database, map them to the User domain object, and return them
    /// </summary>
    /// <returns>
    /// A collection of User objects.
    /// </returns>
    public IEnumerable<User> FetchAll(DtoInputFilteringUsers? dtoInputFilteringUsers)
    {
        FilteringUser? filteringUser = null;
        
        if (dtoInputFilteringUsers != null)
        {
            var roles = _roleRepository.FetchAll();

            filteringUser = new FilteringUser
            {
                Limit = dtoInputFilteringUsers.Limit,
                Offset = dtoInputFilteringUsers.Offset,
                RoleId = roles.FirstOrDefault(role => role.Name == dtoInputFilteringUsers.Role)?.Id,
                Search = dtoInputFilteringUsers.Search
            };
        }

        var dbUsers = _userRepository.FetchAll(filteringUser);
        var users = dbUsers.Select(MapToUser);

        return users;
    }

    /// <summary>
    /// Map the DbUser to a User and then set the Role property of the User to a Role object that is fetched from the
    /// RoleRepository
    /// </summary>
    /// <param name="dbUser">The database entity that we are mapping from.</param>
    /// <returns>
    /// A User object
    /// </returns>
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

    public User ChangeRole(int userId, int roleId)
    {
        var user = FetchById(userId);
        var dbUser = Mapper.GetInstance().Map<DbUser>(user);

        dbUser.RoleId = roleId;

        return MapToUser(_userRepository.Update(dbUser)) ;
    }
}
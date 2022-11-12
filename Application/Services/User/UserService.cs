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
        var user = Mapper.GetInstance().Map<Domain.User>(dbUser);
        
        user.RoleName = _roleRepository.FetchById(user.RoleId).Name;
        return user;
    }

    public User FetchById(int id)
    {
        var dbUser = _userRepository.FetchById(id);
        var user =  Mapper.GetInstance().Map<Domain.User>(dbUser);

        user.RoleName = _roleRepository.FetchById(user.RoleId).Name;
        return user ;
    }

    public IEnumerable<User> FetchAll()
    {
        var dbUsers = _userRepository.FetchAll();
        var users = dbUsers.Select(dbUser => new User
        {
            Id = dbUser.Id,
            FirstName = dbUser.FirstName,
            LastName = dbUser.LastName,
            AccountCreation = dbUser.AccountCreation,
            Email = dbUser.Email,
            RoleId = dbUser.RoleId,
            RoleName = _roleRepository.FetchById(dbUser.RoleId).Name,
            ProfilePicturePath = dbUser.ProfilePicturePath
        });

        return users;
    }
}
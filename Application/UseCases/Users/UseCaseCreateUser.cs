using Application.Services.Auth;
using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;

namespace Application.UseCases.Users;

public class UseCaseCreateUser: IUseCaseWriter<DtoOutputUser?, DtoInputCreateUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UseCaseCreateUser(IUserRepository userRepository, IUserService userService, IAuthService authService)
    {
        _userRepository = userRepository;
        _userService = userService;
        _authService = authService;
    }

    public DtoOutputUser? Execute(DtoInputCreateUser input)
    {
        //Check if the email isn't already used
        try
        {
            _userService.FetchByEmail(input.Email) ;
            throw new Exception("Cet email est déjà utilisé");
        }
        catch (KeyNotFoundException e)
        {
            DbUser user = Mapper.GetInstance().Map<DbUser>(input);
            user.AccountCreation = DateTime.Now;
            user.RoleId = 1 ;
            //hash password
            user.Password = _authService.HashPassword(user.Password);
            var userInDb = _userRepository.Create(user);
            return Mapper.GetInstance().Map<DtoOutputUser>(userInDb);
        }
    }
}
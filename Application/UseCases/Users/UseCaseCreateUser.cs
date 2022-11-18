using Application.Services.Auth;
using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository;

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
            throw new Exception("Cette adresse email est déjà utilisée");
        }
        
        //If the email isn't used
        catch (KeyNotFoundException e)
        {
            DbUser userToAdd = Mapper.GetInstance().Map<DbUser>(input);
            userToAdd.AccountCreation = DateTime.Now;
            userToAdd.RoleId = 1 ;
            //hash password
            userToAdd.Password = _authService.HashPassword(userToAdd.Password);
            
            var newUser = _userRepository.Create(userToAdd);

            var user = _userService.MapToUser(newUser);  
            
            return Mapper.GetInstance().Map<DtoOutputUser>(user) ;
        }
    }
}
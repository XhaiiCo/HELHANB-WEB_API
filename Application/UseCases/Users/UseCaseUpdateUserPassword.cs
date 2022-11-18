using Application.Services.Auth;
using Application.Services.User;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;

namespace Application.UseCases.Users.Dtos;

public class UseCaseUpdatePasswordUser: IUseCaseWriter<DtoOutputUser, DtoInputUpdatePasswordUser>
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public UseCaseUpdatePasswordUser(IUserService userService, IAuthService authService, IUserRepository userRepository)
    {
        _userService = userService;
        _authService = authService;
        _userRepository = userRepository;
    }

    public DtoOutputUser Execute(DtoInputUpdatePasswordUser input)
    {
        //Get user
        var user = _userService.FetchById(input.Id);

        //Hash password
        var newHashedPassword = _authService.HashPassword(input.Password) ;
        
        //Update user
        user.Password = newHashedPassword;

        //Map to dbUser
        var dbUser = Mapper.GetInstance().Map<DbUser>(user) ;

        var updatedDbUser = _userService.MapToUser(_userRepository.Update(dbUser));

        return Mapper.GetInstance().Map<DtoOutputUser>(updatedDbUser);
    }
}
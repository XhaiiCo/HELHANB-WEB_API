using Application.Services.Auth;
using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Users;

public class UseCaseLoginUser: IUseCaseParameterizedQuery<DtoOutputUser, DtoInputLoginUser>
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UseCaseLoginUser(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    public DtoOutputUser Execute(DtoInputLoginUser input)
    {
        User user = _userService.FetchByEmail(input.Email);
       
        if (_authService.ValidatePassword(input.Password, user.Password))
        {
            DtoOutputUser dtoUser = Mapper.GetInstance().Map<DtoOutputUser>(user);
            return dtoUser;
        }

        throw new KeyNotFoundException($"Mot de passe incorrect");

    }
}
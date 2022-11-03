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

    public UseCaseCreateUser(IUserRepository userRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    public DtoOutputUser? Execute(DtoInputCreateUser input)
    {
        //Check if the email isn't already used
        try
        {
            _userService.FetchByEmail(input.Email) ;
            return null;
        }
        catch (KeyNotFoundException e)
        {
            DbUser user = Mapper.GetInstance().Map<DbUser>(input);
            user.AccountCreation = DateTime.Now;
            user.RoleId = 1 ;
            var userInDb = _userRepository.Create(user);
            return Mapper.GetInstance().Map<DtoOutputUser>(userInDb);
        }
    }
}
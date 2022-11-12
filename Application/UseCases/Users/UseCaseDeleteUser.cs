using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;

namespace Application.UseCases.Users;

public class UseCaseDeleteUserById: IUseCaseParameterizedQuery<DtoOutputUser, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public UseCaseDeleteUserById(IUserRepository userRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    public DtoOutputUser Execute(int id)
    {
        var user = _userService.FetchById(id);

        var dbUser = Mapper.GetInstance().Map<DbUser>(user);

        _userRepository.Delete(dbUser);

        return Mapper.GetInstance().Map<DtoOutputUser>(user) ;
    }
}
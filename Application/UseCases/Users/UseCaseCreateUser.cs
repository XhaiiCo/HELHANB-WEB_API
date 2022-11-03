using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;

namespace Application.UseCases.Users;

public class UseCaseCreateUser: IUseCaseWriter<DtoOutputUser, DtoInputCreateUser>
{
    private readonly IUserRepository _userRepository;

    public UseCaseCreateUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public DtoOutputUser Execute(DtoInputCreateUser input)
    {
        DbUser user = Mapper.GetInstance().Map<DbUser>(input);
        user.AccountCreation = DateTime.Now;
        user.RoleId = 1 ;
        var userInDb = _userRepository.Create(user);
        return Mapper.GetInstance().Map<DtoOutputUser>(userInDb);
    }
}
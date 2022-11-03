using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;

namespace Application.UseCases.Users;

public class UseCaseFetchAllUsers: IUseCaseQuery<IEnumerable<DtoOutputUser>>
{
    private readonly IUserRepository _userRepository;

    public UseCaseFetchAllUsers(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IEnumerable<DtoOutputUser> Execute()
    {
        var dbUsers = _userRepository.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputUser>>(dbUsers);
    }
}
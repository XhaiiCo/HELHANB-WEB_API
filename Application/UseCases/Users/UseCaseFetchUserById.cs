using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;

namespace Application.UseCases.Users;

public class UseCaseFetchUserById: IUseCaseParameterizedQuery<DtoOutputUser, int>
{
    private readonly IUserRepository _userRepository;

    public UseCaseFetchUserById(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public DtoOutputUser Execute(int id)
    {
        var userDb = _userRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputUser>(userDb);
    }
}
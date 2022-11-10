using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Microsoft.VisualBasic.CompilerServices;

namespace Application.UseCases.Users;

public class UseCaseFetchUserById: IUseCaseWriter<DtoOutputUser, int>
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
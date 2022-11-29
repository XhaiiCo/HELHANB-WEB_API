using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;
using Microsoft.IdentityModel.Tokens;

namespace Application.UseCases.Users;

public class UseCaseHasHostRenting : IUseCaseWriter<bool, int>
{
    private readonly IAdRepository _adRepository;
    
    public UseCaseHasHostRenting(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }
    
    public bool Execute(int id)
    {
        return !_adRepository.FetchByUserId(id).IsNullOrEmpty();
    }
}
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseCountValidatedAds:IUseCaseQuery<int>
{
    private readonly IAdRepository _adRepository;

    public UseCaseCountValidatedAds(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public int Execute()
    {
        var filter = new FilteringAd
        {
            StatusId = 3
        };
        
        return _adRepository.Count(filter);
    }
}
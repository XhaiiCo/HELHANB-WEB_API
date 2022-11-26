using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseCountAds:IUseCaseQuery<int>
{
    private readonly IAdRepository _adRepository;

    public UseCaseCountAds(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public int Execute()
    {
        return _adRepository.CountValidatedAd();
    }
}
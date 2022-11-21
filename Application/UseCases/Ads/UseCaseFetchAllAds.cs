using Application.Services.Ad;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;

namespace Application.UseCases.Ads;

public class UseCaseFetchAllAds: IUseCaseQuery<IEnumerable<DtoOutputAd>>
{
    private readonly IAdService _adService;

    public UseCaseFetchAllAds (IAdService adService)
    {
        _adService = adService;
    }

    public IEnumerable<DtoOutputAd> Execute()
    {
        var ads = _adService.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputAd>>(ads);
    }
}
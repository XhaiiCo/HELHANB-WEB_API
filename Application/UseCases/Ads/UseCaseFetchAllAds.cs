using Application.Services.Ad;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;

namespace Application.UseCases.Ads;

public class UseCaseFetchAllAds: IUseCaseParameterizedQuery<IEnumerable<DtoOutputAd>, DtoInputFilteringAds>
{
    private readonly IAdService _adService;

    public UseCaseFetchAllAds (IAdService adService)
    {
        _adService = adService;
    }

    public IEnumerable<DtoOutputAd> Execute(DtoInputFilteringAds dtoInputFilteringAds)
    {
        var ads = _adService.FetchAll(dtoInputFilteringAds);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputAd>>(ads);
    }
}
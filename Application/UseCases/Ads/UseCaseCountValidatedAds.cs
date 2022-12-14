using Application.Services.Ad;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseCountValidatedAds : IUseCaseParameterizedQuery<int, DtoInputFilteringAds>
{
    private readonly IAdService _adService;

    public UseCaseCountValidatedAds(IAdService adService)
    {
        _adService = adService;
    }

    public int Execute(DtoInputFilteringAds dto)
    {
        return _adService.FilterAds(Mapper.GetInstance().Map<FilteringAd>(dto)).Count();
    }
}
using Application.Services.Ad;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseFetchAdsForPagination : IUseCaseParameterizedQuery<IEnumerable<DtoOutputAdsSummary>,
    DtoInputFilteringAds>
{
    private readonly IAdService _adService;

    public UseCaseFetchAdsForPagination(IAdService adService)
    {
        _adService = adService;
    }

    public IEnumerable<DtoOutputAdsSummary> Execute(DtoInputFilteringAds dto)
    {
        var ads = _adService.FilterAds(Mapper.GetInstance().Map<FilteringAd>(dto));
        
        var result = Mapper.GetInstance().Map<IEnumerable<DtoOutputAdsSummary>>(ads);
        foreach (var dtoOutputAdsSummary in result)
        {
            var pictures = new List<string>();
            ads.FirstOrDefault(ad => ad.AdSlug == dtoOutputAdsSummary.AdSlug)
                ?.Pictures
                .ForEach(picture => pictures.Add(picture.Path));
            dtoOutputAdsSummary.Pictures = pictures;
        }

        return result;
    }
}
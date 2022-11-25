using Application.Services.Ad;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseFetchAdsForPagination : IUseCaseParameterizedQuery<IEnumerable<DtoOutputAdsSummary>,
    DtoInputFilterAdsForPagination>
{
    private readonly IAdService _adService;

    public UseCaseFetchAdsForPagination(IAdService adService)
    {
        _adService = adService;
    }

    public IEnumerable<DtoOutputAdsSummary> Execute(DtoInputFilterAdsForPagination param)
    {
        var ads = param.Limit.HasValue && param.Offset.HasValue
            ? _adService.FetchRange(param.Offset.Value, param.Limit.Value)
            : _adService.FetchAll();

        var result = Mapper.GetInstance().Map<IEnumerable<DtoOutputAdsSummary>>(ads);
        foreach (var dtoOutputAdsSummary in result)
        {
            var pictures = new List<string>();
            ads.FirstOrDefault(ad => ad.Id == dtoOutputAdsSummary.Id)
                ?.Pictures
                .ForEach(picture => pictures.Add(picture.Path));
            dtoOutputAdsSummary.Pictures = pictures;
        }

        return result;
    }
}
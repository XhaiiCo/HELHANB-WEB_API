using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseCountValidatedAds : IUseCaseParameterizedQuery<int, DtoInputFilteringAds>
{
    private readonly IAdRepository _adRepository;

    public UseCaseCountValidatedAds(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public int Execute(DtoInputFilteringAds dto)
    {
        
        return _adRepository.Count(Mapper.GetInstance().Map<FilteringAd>(dto));
    }
}
using Application.Services.Ad;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseUpdateStatusAd: IUseCaseWriter<DtoOutputAd, DtoInputUpdateStatusAd>
{
    private readonly IAdRepository _adRepository;
    private readonly IAdService _adService ;

    public UseCaseUpdateStatusAd(IAdRepository adRepository, IAdService adService)
    {
        _adRepository = adRepository;
        _adService = adService;
    }

    public DtoOutputAd Execute(DtoInputUpdateStatusAd input)
    {
        var dbAd = _adRepository.FetchBySlug(input.AdSlug);

        dbAd.AdStatusId = input.StatusId;

        var result = _adRepository.Update(dbAd); 
        
        return Mapper.GetInstance().Map<DtoOutputAd>(_adService.MapToAd(result)) ;
    }
}
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseDeleteAd : IUseCaseParameterizedQuery<DtoOutputAd, int>
{
    private readonly IAdRepository _adRepository;
    

    public UseCaseDeleteAd(IAdRepository adRepository)
    {
        _adRepository = adRepository;
        
    }
    
    public DtoOutputAd Execute(int id)
    {
        var ad = _adRepository.FetchById(id);
        
        var dbAd = Mapper.GetInstance().Map<DbAd>(ad);

        _adRepository.Delete(dbAd);

        return Mapper.GetInstance().Map<DtoOutputAd>(ad);
        
    }
}
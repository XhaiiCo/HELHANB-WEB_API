using Application.UseCases.Ads.Dtos;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseCreateAd
{
    private readonly IAdRepository _adRepository;
    
    public UseCaseCreateAd(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }
    
    public DtoOutputAd Execute(DtoInputCreateAd input)
    {
        var mapper = Mapper.GetInstance();

        var dbAd = mapper.Map<DbAd>(input);
        var newAd = _adRepository.Create(dbAd);
        
        return mapper.Map<DtoOutputAd>(dbAd);
    }
}
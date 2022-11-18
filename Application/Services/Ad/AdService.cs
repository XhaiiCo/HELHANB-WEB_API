using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;

namespace Application.Services.Ad;

public class AdService : IAdService
{
    private readonly IAdRepository _adRepository;

    public AdService(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public Domain.Ad FetchById(int id)
    {
        var dbAd = _adRepository.FetchById(id);
        return MapToAd(dbAd);
    }
    
    public Domain.Ad MapToAd(DbAd dbAd)
    {
        var ad = Mapper.GetInstance().Map<Domain.Ad>(dbAd);
        return ad;
    }
}
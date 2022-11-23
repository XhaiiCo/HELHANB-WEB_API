using Application.Services.User;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;

namespace Application.Services.Ad;

public class AdService : IAdService
{
    private readonly IAdRepository _adRepository;
    private readonly IUserService _userService;

    public AdService(IAdRepository adRepository, IUserService userService)
    {
        _adRepository = adRepository;
        _userService = userService;
    }

    public Domain.Ad FetchById(int id)
    {
        var dbAd = _adRepository.FetchById(id);
        return MapToAd(dbAd);
    }

    public IEnumerable<Domain.Ad> FetchAll()
    {
        var dbAds = _adRepository.FetchAll();
        var ads = dbAds.Select(MapToAd);
        
        return ads;
    }

    public Domain.Ad MapToAd(DbAd dbAd)
    {
        var ad = Mapper.GetInstance().Map<Domain.Ad>(dbAd);
        ad.Owner = _userService.FetchById(dbAd.UserId);
        
        return ad;
    }
}
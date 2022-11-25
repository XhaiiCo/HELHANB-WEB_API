using Application.Services.User;
using Domain;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.AdPicture;
using Infrastructure.Ef.Repository.HouseFeature;

namespace Application.Services.Ad;

public class AdService : IAdService
{
    private readonly IAdRepository _adRepository;
    private readonly IUserService _userService;

    private readonly IHouseFeatureRepository _houseFeatureRepository;
    private readonly IAdPictureRepository _adPictureRepository;

    public AdService(IAdRepository adRepository,
        IUserService userService,
        IHouseFeatureRepository houseFeatureRepository,
        IAdPictureRepository adPictureRepository)
    {
        _adRepository = adRepository;
        _userService = userService;
        _houseFeatureRepository = houseFeatureRepository;
        _adPictureRepository = adPictureRepository;
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
    
    public IEnumerable<Domain.Ad> FetchRange(int offset, int limit)
    {
        var dbAds = _adRepository.FetchRange(offset, limit);
        var ads = dbAds.Select(MapToAd);

        return ads;
    }

    public Domain.Ad MapToAd(DbAd dbAd)
    {
        var ad = Mapper.GetInstance().Map<Domain.Ad>(dbAd);
        ad.Owner = _userService.FetchById(dbAd.UserId);

        foreach (var dbHouseFeature in _houseFeatureRepository.FetchByAdId(ad.Id))
        {
            ad.AddFeature(dbHouseFeature.Feature);
        }
        
        foreach (var dbAdPicture in _adPictureRepository.FetchByAdId(ad.Id))
        {
            ad.AddPicture(Mapper.GetInstance().Map<Picture>(dbAdPicture));
        }
        
        return ad;
    }
}
using Application.Services.User;
using Application.UseCases.Ads.Dtos;
using Domain;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.Ad.AdStatus;
using Infrastructure.Ef.Repository.AdPicture;
using Infrastructure.Ef.Repository.HouseFeature;

namespace Application.Services.Ad;

public class AdService : IAdService
{
    private readonly IAdRepository _adRepository;
    private readonly IUserService _userService;

    private readonly IHouseFeatureRepository _houseFeatureRepository;
    private readonly IAdPictureRepository _adPictureRepository;
    private readonly IAdStatusRepository _adStatusRepository;

    public AdService(IAdRepository adRepository,
        IUserService userService,
        IHouseFeatureRepository houseFeatureRepository,
        IAdPictureRepository adPictureRepository, IAdStatusRepository adStatusRepository)
    {
        _adRepository = adRepository;
        _userService = userService;
        _houseFeatureRepository = houseFeatureRepository;
        _adPictureRepository = adPictureRepository;
        _adStatusRepository = adStatusRepository;
    }

    public Domain.Ad FetchById(int id)
    {
        var dbAd = _adRepository.FetchById(id);
        return MapToAd(dbAd);
    }

    public Domain.Ad FetchBySlug(string slug)
    {
        var dbAd = _adRepository.FetchBySlug(slug);
        return MapToAd(dbAd);
    }

    public IEnumerable<Domain.Ad> FetchByUserId(int id)
    {
        var dbAds = _adRepository.FetchByUserId(id);
        
        return dbAds.Select(MapToAd) ;
    }

    public IEnumerable<Domain.Ad> FetchAll(DtoInputFilteringAds dto)
    {
        var dbAds = _adRepository.FetchAll(Mapper.GetInstance().Map<FilteringAd>(dto));
        var ads = dbAds.Select(MapToAd);

        return ads;
    }

    public IEnumerable<Domain.Ad> FetchRange(DtoInputFilteringAds filter)
    {
        var dbAds = _adRepository.FetchRange(Mapper.GetInstance().Map<FilteringAd>(filter));
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

        ad.Status = Mapper.GetInstance().Map<AdStatus>(_adStatusRepository.FetchById(dbAd.AdStatusId));
        return ad;
    }
}
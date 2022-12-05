using API.Utils.Picture;
using Application.Services.Time;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.AdPicture;
using Infrastructure.Ef.Repository.HouseFeature;

namespace Application.UseCases.Ads;

public class UseCaseUpdateAd : IUseCaseWriter<DtoOutputAd, DtoInputUpdateAd>
{
    private readonly IAdRepository _adRepository;
    private readonly ITimeService _timeService;
    private readonly IHouseFeatureRepository _houseFeatureRepository;
    private readonly IAdPictureRepository _adPictureRepository;
    private readonly IPictureService _pictureService;

    public UseCaseUpdateAd(IAdRepository adRepository, ITimeService timeService, IHouseFeatureRepository houseFeatureRepository, IAdPictureRepository adPictureRepository, IPictureService pictureService)
    {
        _adRepository = adRepository;
        _timeService = timeService;
        _houseFeatureRepository = houseFeatureRepository;
        _adPictureRepository = adPictureRepository;
        _pictureService = pictureService;
    }

    public DtoOutputAd Execute(DtoInputUpdateAd input)
    {
        var mapper = Mapper.GetInstance();
        
        var dbAd = _adRepository.FetchById(input.Id);

        dbAd.Name = input.Name;
        dbAd.NumberOfPersons = input.NumberOfPersons;
        dbAd.NumberOfBedrooms = input.NumberOfBedrooms;
        dbAd.Description = input.Description;
        dbAd.PricePerNight = input.PricePerNight;
        dbAd.ArrivalTimeRangeStart = _timeService.ToTimeSpan(input.ArrivalTimeRangeStart);
        dbAd.ArrivalTimeRangeEnd = _timeService.ToTimeSpan(input.ArrivalTimeRangeEnd);
        dbAd.LeaveTime = _timeService.ToTimeSpan(input.LeaveTime);

        var result = _adRepository.Update(dbAd);

        //features
        var dbFeatures = _houseFeatureRepository.FetchByAdId(input.Id);
        
        foreach (var dbFeature in dbFeatures)
        {
                    //because input.Features is like the updated list
            if (!input.Features.Contains(dbFeature.Feature))
            {
                _houseFeatureRepository.Delete(dbFeature);
            }
        }
        
        var dbFeaturesFeatures = _houseFeatureRepository.FetchByAdId(input.Id).Select(dbFeature => dbFeature.Feature);

        var diff = input.Features.Except(dbFeaturesFeatures);

        foreach (var newFeature in diff)
        {
            _houseFeatureRepository.Create(new DbHouseFeature
            {
                Feature = newFeature,
                AdId = input.Id
            });
        }
        
        //pictures
        var dbAdPictures = _adPictureRepository.FetchByAdId(input.Id);

        foreach (var dbAdPicture in dbAdPictures)
        {
            if (input.PicturesToDelete.Contains(dbAdPicture.Path))
            {
                _adPictureRepository.Delete(dbAdPicture);
                
                _pictureService.RemoveFile(dbAdPicture.Path);
            }
        }
        
        
        return mapper.Map<DtoOutputAd>(result);
    }
}
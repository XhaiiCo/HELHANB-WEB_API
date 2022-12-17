using API.Services;
using API.Utils.Picture;
using Application.Services.Ad;
using Application.Services.Time;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Domain;
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
    private readonly ISlugService _slugService;
    public readonly IAdService _adService;

    public UseCaseUpdateAd(IAdRepository adRepository, ITimeService timeService,
        IHouseFeatureRepository houseFeatureRepository, IAdPictureRepository adPictureRepository,
        IPictureService pictureService, ISlugService slugService, IAdService adService)
    {
        _adRepository = adRepository;
        _timeService = timeService;
        _houseFeatureRepository = houseFeatureRepository;
        _adPictureRepository = adPictureRepository;
        _pictureService = pictureService;
        _slugService = slugService;
        _adService = adService;
    }

    public DtoOutputAd Execute(DtoInputUpdateAd input)
    {
        var mapper = Mapper.GetInstance();

        var dbAd = _adRepository.FetchBySlug(input.AdSlug);
        var dbAdPictures = _adPictureRepository.FetchByAdId(dbAd.Id);

        //Validations images
        var size = dbAdPictures.Count() + input.PicturesToAdd.Count() - input.PicturesToDelete.Count();
        if (size is < 3 or > 15)
        {
            throw new Exception("Le nombre d'images doit être compris entre 3 et 15");
        }

        if (input.PicturesToAdd.Any() && _pictureService.ValidExtensions(input.PicturesToAdd))
        {
            throw new Exception("Les seuls formats de fichiers acceptés sont : jpeg, png, webp");
        }

        dbAd.Name = input.Name;
        dbAd.NumberOfPersons = input.NumberOfPersons;
        dbAd.NumberOfBedrooms = input.NumberOfBedrooms;
        dbAd.Description = input.Description;
        dbAd.PricePerNight = input.PricePerNight;
        dbAd.ArrivalTimeRangeStart = _timeService.ToTimeSpan(input.ArrivalTimeRangeStart);
        dbAd.ArrivalTimeRangeEnd = _timeService.ToTimeSpan(input.ArrivalTimeRangeEnd);
        dbAd.LeaveTime = _timeService.ToTimeSpan(input.LeaveTime);

        //Validation hours
        Ad.ValidHours(dbAd.ArrivalTimeRangeStart, dbAd.ArrivalTimeRangeEnd, dbAd.LeaveTime);


        var result = _adRepository.Update(dbAd);

        //features
        var dbFeatures = _houseFeatureRepository.FetchByAdId(dbAd.Id);

        foreach (var dbFeature in dbFeatures)
        {
            //because input.Features is like the updated list
            if (!input.Features.Contains(dbFeature.Feature))
            {
                _houseFeatureRepository.Delete(dbFeature);
            }
        }

        var dbFeaturesFeatures = _houseFeatureRepository.FetchByAdId(dbAd.Id).Select(dbFeature => dbFeature.Feature);

        var diff = input.Features.Except(dbFeaturesFeatures);

        foreach (var newFeature in diff)
        {
            _houseFeatureRepository.Create(new DbHouseFeature
            {
                Feature = newFeature,
                AdId = dbAd.Id
            });
        }

        //pictures

        foreach (var dbAdPicture in dbAdPictures)
        {
            if (input.PicturesToDelete.Contains(dbAdPicture.Path))
            {
                _adPictureRepository.Delete(dbAdPicture);

                _pictureService.RemoveFile(dbAdPicture.Path);
            }
        }

        var basePath = "\\Upload\\AdPictures\\" + dbAd.Id + "\\";
        var filePath = "";

        var existingPics = dbAdPictures.Select(dbAdPicture => _pictureService.PathToBytes(dbAdPicture.Path));

        foreach (var pic in input.PicturesToAdd)
        {
            //on vérifie qu on essaye pas de remettre une deuxieme fois la meme image
            if (_pictureService.ContainsImage(existingPics, _pictureService.Base64ToBytes(pic))) continue;

            filePath = basePath + _pictureService.GenerateUniqueFileName(dbAd.UserId) +
                       _pictureService.GetExtension(pic);

            var dtoInputAddPictureAd = new DtoInputAddPictureAd
            {
                Path = filePath,
                AdId = dbAd.Id
            };
            var dbAdPicture = Mapper.GetInstance().Map<DbAdPicture>(dtoInputAddPictureAd);

            _adPictureRepository.Create(dbAdPicture);

            _pictureService.UploadBase64Picture(basePath, filePath, pic);
        }

        return mapper.Map<DtoOutputAd>(_adService.MapToAd(result));
    }
}
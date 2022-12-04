using API.Utils.Picture;
using Application.Services.Time;
using Application.Services.User;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.AdPicture;
using Infrastructure.Ef.Repository.HouseFeature;

namespace Application.UseCases.Ads;

public class UseCaseCreateAd : IUseCaseWriter<DtoOutputAd, DtoInputCreateAd>
{
    private readonly IAdRepository _adRepository;
    private readonly IHouseFeatureRepository _houseFeatureRepository;
    private readonly IUserService _userService;
    private readonly ITimeService _timeService;
    private readonly IPictureService _pictureService;
    private readonly IAdPictureRepository _adPictureRepository;

    public UseCaseCreateAd(IAdRepository adRepository, IUserService userService, ITimeService timeService,
        IHouseFeatureRepository houseFeatureRepository, IPictureService pictureService, IAdPictureRepository adPictureRepository)
    {
        _adRepository = adRepository;
        _userService = userService;
        _timeService = timeService;
        _houseFeatureRepository = houseFeatureRepository;
        _pictureService = pictureService;
        _adPictureRepository = adPictureRepository;
    }

    public DtoOutputAd Execute(DtoInputCreateAd input)
    {
        var mapper = Mapper.GetInstance();

        var user = _userService.FetchById(input.UserId);

        if (user.Role.Id != 2)
            throw new UnauthorizedAccessException("L'utilisateur doit être un hôte");

        //on fait déjà une verif pour les images ici
        if (_pictureService.ValidExtensions(input.PicturesToAdd))
        {
            throw new Exception("Les seuls formats de fichiers acceptés sont : jpeg, png, webp");
        }
        
        //Add the ad
        var dbAd = mapper.Map<DbAd>(input);
        dbAd.Created = DateTime.Now;
        dbAd.AdStatusId = 1;

        //Time
        dbAd.ArrivalTimeRangeStart = _timeService.ToTimeSpan(input.ArrivalTimeRangeStart);
        dbAd.ArrivalTimeRangeEnd = _timeService.ToTimeSpan(input.ArrivalTimeRangeEnd);
        dbAd.LeaveTime = _timeService.ToTimeSpan(input.LeaveTime);

        Ad.ValidHours(dbAd.ArrivalTimeRangeStart, dbAd.ArrivalTimeRangeEnd, dbAd.LeaveTime);

        var newAd = _adRepository.Create(dbAd);
        
        //Add the features
        foreach (var inputFeature in input.Features)
        {
            var dbHouseFeature = new DbHouseFeature
            {
                Feature = inputFeature,
                AdId = newAd.Id
            };
            _houseFeatureRepository.Create(dbHouseFeature);
        }

        var basePath = "\\Upload\\AdPictures\\" + newAd.Id + "\\";
        var fullpath = "";

        
        foreach (var pic in input.PicturesToAdd)
        {
            fullpath = basePath + _pictureService.GenerateUniqueFileName(newAd.UserId) + _pictureService.GetExtension(pic);
            
            var dtoInputAddPictureAd = new DtoInputAddPictureAd
            {
                Path = fullpath,
                AdId = newAd.Id
            };
            var dbAdPicture = Mapper.GetInstance().Map<DbAdPicture>(dtoInputAddPictureAd);
            
            _adPictureRepository.Create(dbAdPicture);
            
            _pictureService.UploadBase64Picture(basePath, fullpath, pic);
        }


        return mapper.Map<DtoOutputAd>(newAd);
    }
}
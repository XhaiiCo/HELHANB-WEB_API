using API.Services;
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
    private readonly ISlugService _slugService;

    public UseCaseCreateAd(IAdRepository adRepository, IUserService userService, ITimeService timeService,
        IHouseFeatureRepository houseFeatureRepository, IPictureService pictureService, IAdPictureRepository adPictureRepository, ISlugService slugService)
    {
        _adRepository = adRepository;
        _userService = userService;
        _timeService = timeService;
        _houseFeatureRepository = houseFeatureRepository;
        _pictureService = pictureService;
        _adPictureRepository = adPictureRepository;
        _slugService = slugService;
    }

    public DtoOutputAd Execute(DtoInputCreateAd input)
    {
        var mapper = Mapper.GetInstance();

        var user = _userService.FetchById(input.UserId);

        if (user.Role.Id != 2)
            throw new UnauthorizedAccessException("L'utilisateur doit être un hôte");

        var size = input.PicturesToAdd.ToArray().Length;
        if (size < 3 || size > 15)
        {
            throw new Exception("Le nombre d'images doit être compris entre 3 et 15");
        }
        
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

        dbAd.AdSlug = _slugService.GenerateSlug(input.Name);
        
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
        var filePath = "";
        
        foreach (var pic in input.PicturesToAdd)
        {
            filePath = basePath + _pictureService.GenerateUniqueFileName(newAd.UserId) + _pictureService.GetExtension(pic);
            
            var dtoInputAddPictureAd = new DtoInputAddPictureAd
            {
                Path = filePath,
                AdId = newAd.Id
            };
            var dbAdPicture = Mapper.GetInstance().Map<DbAdPicture>(dtoInputAddPictureAd);
            
            _adPictureRepository.Create(dbAdPicture);
            
            _pictureService.UploadBase64Picture(basePath, filePath, pic);
        }

        

        return mapper.Map<DtoOutputAd>(newAd);
    }
}
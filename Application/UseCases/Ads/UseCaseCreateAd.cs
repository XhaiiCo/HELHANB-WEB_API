using Application.Services.Time;
using Application.Services.User;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.HouseFeature;

namespace Application.UseCases.Ads;

public class UseCaseCreateAd : IUseCaseWriter<DtoOutputAd, DtoInputCreateAd>
{
    private readonly IAdRepository _adRepository;
    private readonly IHouseFeatureRepository _houseFeatureRepository;
    private readonly IUserService _userService;
    private readonly ITimeService _timeService;

    public UseCaseCreateAd(IAdRepository adRepository, IUserService userService, ITimeService timeService,
        IHouseFeatureRepository houseFeatureRepository)
    {
        _adRepository = adRepository;
        _userService = userService;
        _timeService = timeService;
        _houseFeatureRepository = houseFeatureRepository;
    }

    public DtoOutputAd Execute(DtoInputCreateAd input)
    {
        var mapper = Mapper.GetInstance();

        var user = _userService.FetchById(input.UserId);

        if (user.Role.Id != 2)
            throw new UnauthorizedAccessException("L'utilisateur doit être un hôte");

        //Add the ad
        var dbAd = mapper.Map<DbAd>(input);
        dbAd.Created = DateTime.Now;
        dbAd.AdStatusId = 1;

        //Time
        dbAd.ArrivalTimeRangeStart = _timeService.ToTimeSpan(input.ArrivalTimeRangeStart);
        dbAd.ArrivalTimeRangeEnd = _timeService.ToTimeSpan(input.ArrivalTimeRangeEnd);
        dbAd.LeaveTime = _timeService.ToTimeSpan(input.LeaveTime);

        Ad.validHours(dbAd.ArrivalTimeRangeStart, dbAd.ArrivalTimeRangeEnd, dbAd.LeaveTime);

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

        return mapper.Map<DtoOutputAd>(newAd);
    }
}
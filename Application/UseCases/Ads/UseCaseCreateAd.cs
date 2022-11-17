using Application.Services.Time;
using Application.Services.User;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseCreateAd : IUseCaseWriter<DtoOutputAd, DtoInputCreateAd>
{
    private readonly IAdRepository _adRepository;
    private readonly IUserService _userService;
    private readonly ITimeService _timeService;
    
    public UseCaseCreateAd(IAdRepository adRepository, IUserService userService, ITimeService timeService)
    {
        _adRepository = adRepository;
        _userService = userService;
        _timeService = timeService;
    }
    
    public DtoOutputAd Execute(DtoInputCreateAd input)
    {
        var mapper = Mapper.GetInstance();

        var user = _userService.FetchById(input.UserId);

        if (user.Role.Id != 2)
            throw new UnauthorizedAccessException("L'utilisateur doit être un hôte");
           
        
        var dbAd = mapper.Map<DbAd>(input);
        dbAd.Created = DateTime.Now;
        dbAd.AdStatusId = 1;
        
        //Time
        dbAd.ArrivalTimeRangeStart = _timeService.ToTimeSpan(input.ArrivalTimeRangeStart);
        dbAd.ArrivalTimeRangeEnd = _timeService.ToTimeSpan(input.ArrivalTimeRangeEnd);
        dbAd.LeaveTime = _timeService.ToTimeSpan(input.LeaveTime);
        
        var newAd = _adRepository.Create(dbAd);
        
        return mapper.Map<DtoOutputAd>(newAd);
    }
}
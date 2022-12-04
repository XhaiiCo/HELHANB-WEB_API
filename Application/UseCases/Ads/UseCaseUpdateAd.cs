using Application.Services.Time;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseUpdateAd : IUseCaseWriter<DtoOutputAd, DtoInputUpdateAd>
{
    private readonly IAdRepository _adRepository;
    private readonly ITimeService _timeService;

    public UseCaseUpdateAd(IAdRepository adRepository, ITimeService timeService)
    {
        _adRepository = adRepository;
        _timeService = timeService;
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
        
        return mapper.Map<DtoOutputAd>(result);
    }
}
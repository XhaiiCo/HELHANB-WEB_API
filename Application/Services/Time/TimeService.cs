using Application.UseCases.Ads.Dtos;

namespace Application.Services.Time;

public class TimeService: ITimeService
{
    public TimeSpan ToTimeSpan(DtoInputTime time)
    {
        return new TimeSpan(time.Hours, time.Minutes, 0);
    }

}
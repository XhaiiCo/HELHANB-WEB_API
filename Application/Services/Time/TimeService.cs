using Application.UseCases.Ads.Dtos;

namespace Application.Services.Time;

public class TimeService : ITimeService
{
    /// <summary>
    /// It takes a DtoInputTime object and returns a TimeSpan object
    /// </summary>
    /// <returns>
    /// A TimeSpan object.
    /// </returns>
    public TimeSpan ToTimeSpan(DtoInputTime time)
    {
        return new TimeSpan(time.Hours, time.Minutes, 0);
    }
}
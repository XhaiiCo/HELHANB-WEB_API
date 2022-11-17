using Application.UseCases.Ads.Dtos;

namespace Application.Services.Time;

public interface ITimeService
{

    public TimeSpan ToTimeSpan(DtoInputTime time);
}
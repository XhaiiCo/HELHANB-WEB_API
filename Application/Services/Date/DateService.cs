using Application.UseCases.Reservations.Dtos;

namespace Application.Services.Date;

public class DateService: IDateService
{
    public DateTime DateAndTimeCombineur(DateOnly dateOnly, TimeSpan timeSpan)
    {
        return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day,
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    public DateOnly MapToDateOnly(DtoInputDateOnly dtoDateOnly)
    {
        return new DateOnly(dtoDateOnly.Year, dtoDateOnly.Month, dtoDateOnly.Day);
    }
}
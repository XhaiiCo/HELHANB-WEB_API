using Application.UseCases.Reservations.Dtos;

namespace Application.Services.Date;

public class DateService: IDateService
{
    /// <summary>
    /// It takes a DateOnly object and a TimeSpan object and returns a DateTime object
    /// </summary>
    /// <returns>
    /// A DateTime object
    /// </returns>
    public DateTime DateAndTimeCombiner(DateOnly dateOnly, TimeSpan timeSpan)
    {
        return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day,
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    /// <summary>
    /// Map a DTO to a DateOnly object
    /// </summary>
    /// <param name="dtoDateOnly">The DTO that is being passed in.</param>
    /// <returns>
    /// A new instance of the DateOnly class.
    /// </returns>
    public DateOnly MapToDateOnly(DtoInputDateOnly dtoDateOnly)
    {
        return new DateOnly(dtoDateOnly.Year, dtoDateOnly.Month, dtoDateOnly.Day);
    }

    public DtoInputDateOnly MapToDtoInputDateOnly(DateTime dateTime)
    {
        return new DtoInputDateOnly { Day = dateTime.Day, Month = dateTime.Month, Year = dateTime.Year };
    }
}
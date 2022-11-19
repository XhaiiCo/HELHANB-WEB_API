using Application.UseCases.Reservations.Dtos;

namespace Application.Services.Date;

public interface IDateService
{

    public DateTime DateAndTimeCombineur(DateOnly dateOnly, TimeSpan timeSpan);
    public DateOnly MapToDateOnly(DtoInputDateOnly dtoDateOnly);
}
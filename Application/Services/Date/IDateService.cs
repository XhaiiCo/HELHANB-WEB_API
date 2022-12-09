using Application.UseCases.Reservations.Dtos;

namespace Application.Services.Date;

public interface IDateService
{

    public DateTime DateAndTimeCombiner(DateOnly dateOnly, TimeSpan timeSpan);
    public DateOnly MapToDateOnly(DtoInputDateOnly dtoDateOnly);
    public DtoInputDateOnly MapToDtoInputDateOnly(DateTime dateTime);
}
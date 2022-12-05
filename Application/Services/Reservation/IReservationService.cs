using Application.UseCases.Reservations.Dtos;
using Infrastructure.Ef.DbEntities;

namespace Application.Services;

public interface IReservationService
{
  Domain.Reservation MapToReservation(DbReservation dbReservation);
}
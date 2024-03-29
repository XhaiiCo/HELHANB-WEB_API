using Infrastructure.Ef.DbEntities;

namespace Application.Services.Reservation;

public interface IReservationService
{
    public Domain.Reservation MapToReservation(DbReservation dbReservation);
    public DbReservation ConfirmReservation(DbReservation dbReservation);
    public DbReservation RefuseReservation(DbReservation dbReservation);
}
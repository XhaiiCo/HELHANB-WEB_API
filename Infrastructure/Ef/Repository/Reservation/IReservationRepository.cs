using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.Reservation;

public interface IReservationRepository
{
    DbReservation Create(DbReservation reservation);
    IEnumerable<DbReservation> FilterByAdId(int userId);
}
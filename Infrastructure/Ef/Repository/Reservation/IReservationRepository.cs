using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.Reservation;

public interface IReservationRepository
{
    DbReservation Create(DbReservation reservation);
    IEnumerable<DbReservation> FilterByAdId(int adId);
    IEnumerable<DbReservation> FilterByRenterId(int renterId);
    DbReservation FindById(int id);
    DbReservation Delete(DbReservation reservation);
    DbReservation Update(DbReservation reservation);
}
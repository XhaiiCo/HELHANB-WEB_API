using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.AdStatus;

public interface IReservationStatusRepository
{
    DbReservationStatus FetchById(int id);
}
using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository.AdStatus;

public class ReservationStatusRepository:IReservationStatusRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public ReservationStatusRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }
    
    public DbReservationStatus FetchById(int id)
    {
        using var context = _contextProvider.NewContext();
        var dbReservationStatus = context.ReservationStatus.FirstOrDefault(rS => rS.Id == id);
        
        if (dbReservationStatus == null)
            throw new KeyNotFoundException($"Reservation status with id {id} has not been found");

        return dbReservationStatus;
    }
}
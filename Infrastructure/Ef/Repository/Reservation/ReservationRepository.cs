using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository.Reservation;

public class ReservationRepository : IReservationRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public ReservationRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public DbReservation Create(DbReservation reservation)
    {
        using var context = _contextProvider.NewContext();
        context.Reservations.Add(reservation);
        context.SaveChanges();
        return reservation;
    }

    /// <summary>
    /// It returns a list of reservations for a given ad
    /// </summary>
    /// <param name="adId">The id of the ad that we want to filter the reservations by.</param>
    /// <returns>
    /// A list of reservations that match the adId
    /// </returns>
    public IEnumerable<DbReservation> FilterByAdId(int adId)
    {
        using var context = _contextProvider.NewContext();

        var reservations =
            context.Reservations
                .Where(dbR => dbR.AdId == adId)
                .ToList();

        return reservations;
    }

    /// <summary>
    /// It returns a list of reservations for a given renter
    /// </summary>
    /// <param name="renterId">The id of the renter to filter by.</param>
    /// <returns>
    /// A list of reservations
    /// </returns>
    public IEnumerable<DbReservation> FilterByRenterId(int renterId)
    {
        using var context = _contextProvider.NewContext();

        var reservations =
            context.Reservations
                .Where(dbR => dbR.RenterId == renterId)
                .ToList();

        return reservations;
    }

    public DbReservation FindById(int id)
    {
        using var context = _contextProvider.NewContext();
        var reservation = context.Reservations.FirstOrDefault(reservation => reservation.Id == id);

        if (reservation == null)
            throw new KeyNotFoundException($"Reservation with id {id} has not been found");

        return reservation;
    }

    public DbReservation Delete(DbReservation reservation)
    {
        using var context = _contextProvider.NewContext();

        context.Reservations.Remove(reservation);
        context.SaveChanges();

        return reservation;
    }

    public DbReservation Update(DbReservation reservation)
    {
        using var context = _contextProvider.NewContext();

        context.Reservations.Update(reservation);
        context.SaveChanges();

        return reservation;
    }
}
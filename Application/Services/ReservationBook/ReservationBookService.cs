using Domain;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.Services.ReservationBook;

public class ReservationBookService : IReservationBookService
{
    private readonly IReservationRepository _reservationRepository;

    public ReservationBookService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    /// <summary>
    /// It fetches a list of reservations from the database, and then transforms them into a list of domain objects
    /// </summary>
    /// <param name="adId">The id of the ad that we want to fetch the reservations for.</param>
    /// <returns>
    /// A collection of reservations.
    /// </returns>
    public Domain.ReservationBook FetchByAdId(int adId)
    {
        var dbReservations = _reservationRepository.FilterByAdId(adId);

        var reservations = dbReservations.Select(dbReservation => new Reservation
        {
            Id = dbReservation.Id,
            Creation = dbReservation.Creation,
            DateTimeRange = new DateTimeRange
            (
                dbReservation.ArrivalDate,
                dbReservation.LeaveDate
            ),
            ReservationStatus = new ReservationStatus
            {
                Id = dbReservation.ReservationStatusId
            }
        });

        return Domain.ReservationBook.Of(reservations);
    }
}
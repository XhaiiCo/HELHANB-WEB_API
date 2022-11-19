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

    public Domain.ReservationBook Fetch(int adId)
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
            reservationStatus = new ReservationStatus
            {
                Id = dbReservation.ReservationStatusId
            }
        });

        return Domain.ReservationBook.Of(reservations);
    }
}
using Application.Services.Reservation;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.Services.ReservationBook;

public class ReservationBookService : IReservationBookService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationService _reservationService;
    private readonly IAdRepository _adRepository;

    public ReservationBookService(IReservationRepository reservationRepository, IReservationService reservationService, IAdRepository adRepository)
    {
        _reservationRepository = reservationRepository;
        _reservationService = reservationService;
        _adRepository = adRepository;
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

        var reservations = dbReservations.Select(_reservationService.MapToReservation);

        return Domain.ReservationBook.Of(reservations);
    }
    
    public Domain.ReservationBook FetchByAdSlug(string adSlug)
    {
        var adId = _adRepository.FetchBySlug(adSlug).Id;
        return FetchByAdId(adId);
    }
}

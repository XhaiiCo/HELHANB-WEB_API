using Application.Services.Ad;
using Application.Services.ReservationBook;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseFetchAdById : IUseCaseParameterizedQuery<DtoOutputAdWithReservations, int>
{
    private readonly IAdService _adService;
    private readonly IReservationBookService _reservationBookService;

    public UseCaseFetchAdById(IAdService adService, IReservationBookService reservationBookService)
    {
        _adService = adService;
        _reservationBookService = reservationBookService;
    }

    public DtoOutputAdWithReservations Execute(int id)
    {
        var ad = _adService.FetchById(id);

        var dto = Mapper.GetInstance().Map<DtoOutputAdWithReservations>(ad);

        var reservations = _reservationBookService.FetchByAdId(id);

        var reservationsList = reservations.Entries().Select(reservation =>
            new DtoOutputAdWithReservations.DtoOutputAdReservation
            {
                ArrivalDate = reservation.DateTimeRange.ArrivalDate,
                LeaveDate = reservation.DateTimeRange.LeaveDate
            }).ToList();

        dto.Reservations = reservationsList;

        return dto;
    }
}
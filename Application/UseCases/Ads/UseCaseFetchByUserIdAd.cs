using Application.Services.Ad;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Ads;

public class UseCaseFetchByUserIdAd : IUseCaseParameterizedQuery<IEnumerable<DtoOutputMyAdsAd>, int>
{
    private readonly IAdService _adService;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReservationStatusRepository _reservationStatusRepository;

    public UseCaseFetchByUserIdAd(IAdService adService, IReservationRepository reservationRepository, IUserRepository userRepository, IReservationStatusRepository reservationStatusRepository)
    {
        _adService = adService;
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _reservationStatusRepository = reservationStatusRepository;
    }

    public IEnumerable<DtoOutputMyAdsAd> Execute(int id)
    {
        var ads = _adService.FetchByUserId(id);

        var dtos = Mapper.GetInstance().Map<IEnumerable<DtoOutputMyAdsAd>>(ads);

        foreach (var dto in dtos)
        {
            var reservations = _reservationRepository.FilterByAdId(dto.Id);

            var reservationsList = reservations.Select(reservation =>
                new DtoOutputMyAdsAd.DtoOutputAdReservationMyAds
                {
                    ArrivalDate = reservation.ArrivalDate,
                    LeaveDate = reservation.LeaveDate,
                    RenterMyAds = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoRenterMyAds>(_userRepository.FetchById(reservation.RenterId)),
                    StatusMyAds = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoReservationStatusMyAds>(_reservationStatusRepository.FetchById(reservation.ReservationStatusId)),
                }).ToList();

            dto.Reservations = reservationsList;
        }

        return dtos;
    }
}
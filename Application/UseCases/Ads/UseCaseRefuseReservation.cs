using Application.Services.Ad;
using Application.Services.Reservation;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Ads;

public class
    UseCaseRefuseReservation : IUseCaseWriter<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds, DtoInputAdReservationMyAds>
{
    private readonly IUserRepository _userRepository;
    private readonly IReservationStatusRepository _reservationStatusRepository;
    private readonly IReservationService _reservationService;
    private readonly IReservationRepository _reservationRepository;
    private readonly IAdService _adService;

    public UseCaseRefuseReservation(
        IUserRepository userRepository,
        IReservationStatusRepository reservationStatusRepository,
        IReservationService reservationService,
        IReservationRepository reservationRepository, IAdService adService)
    {
        _reservationService = reservationService;
        _userRepository = userRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _reservationRepository = reservationRepository;
        _adService = adService;
    }

    public DtoOutputMyAdsAd.DtoOutputAdReservationMyAds Execute(DtoInputAdReservationMyAds reservation)
    {
        var ad = _adService.FetchBySlug(reservation.adSlug);

        if (ad.Owner.Id != reservation.userId)
            throw new Exception("Vous n'avez pas le droit de modifier cette annonce");

        var dbReservation = _reservationRepository.FindById(reservation.Id);

        _reservationService.RefuseReservation(dbReservation);

        var outputDto = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds>(dbReservation);
        outputDto.StatusMyAds = Mapper.GetInstance()
            .Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoReservationStatusMyAds>(
                _reservationStatusRepository.FetchById(dbReservation.ReservationStatusId));
        outputDto.RenterMyAds = Mapper.GetInstance()
            .Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoRenterMyAds>(
                _userRepository.FetchById(dbReservation.RenterId));

        return outputDto;
    }
}
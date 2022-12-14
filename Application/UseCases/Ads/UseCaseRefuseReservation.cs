using Application.Services.Reservation;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Ads;

public class UseCaseRefuseReservation : IUseCaseWriter<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds, DtoInputAdReservationMyAds>
{
    private readonly IUserRepository _userRepository;
    private readonly IReservationStatusRepository _reservationStatusRepository;
    private readonly IReservationService _reservationService;
    private readonly IReservationRepository _reservationRepository; 

    public UseCaseRefuseReservation(
        IUserRepository userRepository, 
        IReservationStatusRepository reservationStatusRepository, 
        IReservationService reservationService,
        IReservationRepository reservationRepository)
    {
        _reservationService = reservationService;
        _userRepository = userRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _reservationRepository = reservationRepository;
    }
    
    public DtoOutputMyAdsAd.DtoOutputAdReservationMyAds Execute(DtoInputAdReservationMyAds reservation)
    {
        var dbReservation = _reservationRepository.FindById(reservation.Id);

        _reservationService.RefuseReservation(dbReservation);

        var outputDto = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds>(dbReservation);
        outputDto.StatusMyAds = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoReservationStatusMyAds>(_reservationStatusRepository.FetchById(dbReservation.ReservationStatusId));
        outputDto.RenterMyAds = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoRenterMyAds>(_userRepository.FetchById(dbReservation.RenterId));
        
        return outputDto;
    }
}
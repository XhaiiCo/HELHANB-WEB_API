using Application.Services.Ad;
using Application.Services.Reservation;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Reservations;

public class UseCaseConfirmReservation : IUseCaseWriter<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds, DtoInputAdReservationMyAds>
{
    private readonly IAdService _adService;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReservationStatusRepository _reservationStatusRepository;
    private readonly IReservationService _reservationService;
    
    public UseCaseConfirmReservation(
        IAdService adService, 
        IReservationRepository reservationRepository, 
        IUserRepository userRepository, 
        IReservationStatusRepository reservationStatusRepository,
        IReservationService reservationService)
    {
        _adService = adService;
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _reservationService = reservationService;
    }
    
    public DtoOutputMyAdsAd.DtoOutputAdReservationMyAds Execute(DtoInputAdReservationMyAds reservation)
    {
        // Récupérer l'annonce avec la reservationId
        var ad = _adService.FetchBySlug(reservation.adSlug);

        if (ad.Owner.Id != reservation.userId)
            throw new Exception("Vous n'avez pas le droit de modifier cette annonce");
                
        var dbReservation = _reservationRepository.FindById(reservation.Id);

        // Récupérer toutes les réservations en attente et acceptées
        var reservationsList = _reservationRepository.FilterByAdId(ad.Id);

        var conflictsList = Array.Empty<DbReservation>();
        List<DbReservation> tmp;
        
        // Récupère la liste des conflits
        foreach (var reserv in reservationsList)
        {
            if (dbReservation.Id == reserv.Id) continue;
            foreach (var dateI in GetDatesListe(dbReservation.ArrivalDate, dbReservation.LeaveDate))
            {
                foreach (var dateJ in GetDatesListe(reserv.ArrivalDate, reserv.LeaveDate))
                {
                    if (DateTime.Compare(dateI, dateJ) == 0)
                    {
                        tmp = conflictsList.ToList();
                        tmp.Add(reserv);
                        conflictsList = tmp.ToArray();
                        
                        goto Loop;
                    }
                }
                Loop : ;
            }
        }
        
        // Changer le status des réservations refusées 
        foreach (var conflictReservation in conflictsList)
        {
            _reservationService.RefuseReservation(conflictReservation);
        }
        // Changer le status de la réservation confirmée
        _reservationService.ConfirmReservation(dbReservation);

        var outputDto = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds>(dbReservation);
        outputDto.StatusMyAds = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoReservationStatusMyAds>(_reservationStatusRepository.FetchById(dbReservation.ReservationStatusId));
        outputDto.RenterMyAds = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoRenterMyAds>(_userRepository.FetchById(dbReservation.RenterId));
        // Renvoyer le dto
        return outputDto;
    }

    private IEnumerable<DateTime> GetDatesListe(DateTime a, DateTime b)
    {
        a = DateTime.Parse(a.ToString("d"));
        b = DateTime.Parse(b.ToString("d"));
        
        var list = Array.Empty<DateTime>();
        DateTime currDate = a;
        List<DateTime> tmp;
        
        do
        {
            tmp = list.ToList();
            tmp.Add(currDate);
            list = tmp.ToArray();
            
            currDate = currDate.AddDays(1);
            
        } while (DateTime.Compare(currDate, b) != 0);

        return list;
    }
    
}
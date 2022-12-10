using Application.Services;
using Application.Services.Reservation;
using Application.UseCases.Reservations.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.AdPicture;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Reservations;

public class UseCaseFetchMyReservations : IUseCaseParameterizedQuery<IEnumerable<DtoOutputReservation>, int>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationService _reservationService;
    private readonly IAdPictureRepository _adPictureRepository;
    private readonly IAdRepository _adRepository;

    public UseCaseFetchMyReservations(
        IReservationRepository reservationRepository,
        IReservationService reservationService,
        IAdPictureRepository adPictureRepository, IAdRepository adRepository)
    {
        _reservationRepository = reservationRepository;
        _reservationService = reservationService;
        _adPictureRepository = adPictureRepository;
        _adRepository = adRepository;
    }

    public IEnumerable<DtoOutputReservation> Execute(int renterId)
    {
        var dbReservations = _reservationRepository.FilterByRenterId(renterId);

        var dtoReservations = Mapper.GetInstance().Map<IEnumerable<DtoOutputReservation>>(
            dbReservations.Select(_reservationService.MapToReservation)
        );

        foreach (var dtoReservation in dtoReservations)
        {
            var dbReservation = dbReservations.FirstOrDefault(item => item.Id == dtoReservation.Id);

            if (dbReservation != null)
            {
                dtoReservation.ArrivalDate = dbReservation.ArrivalDate;
                dtoReservation.LeaveDate = dbReservation.LeaveDate;
            }

            //Find the adId
            var adId = _adRepository.FetchBySlug(dtoReservation.Ad.AdSlug).Id;
            var pictures = _adPictureRepository.FetchByAdId(adId);
            if (pictures.Any())
            {
                dtoReservation.picture = pictures.ElementAt(0).Path;
            }
        }

        return dtoReservations;
    }
}
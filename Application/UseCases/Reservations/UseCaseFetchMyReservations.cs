using Application.Services;
using Application.UseCases.Reservations.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.AdPicture;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Reservations;

public class UseCaseFetchMyReservations : IUseCaseParameterizedQuery<IEnumerable<DtoOutputReservation>, int>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationService _reservationService;
    private readonly IAdPictureRepository _adPictureRepository;

    public UseCaseFetchMyReservations(
        IReservationRepository reservationRepository,
        IReservationService reservationService,
        IAdPictureRepository adPictureRepository
    )
    {
        _reservationRepository = reservationRepository;
        _reservationService = reservationService;
        _adPictureRepository = adPictureRepository;
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

            var pictures = _adPictureRepository.FetchByAdId(dtoReservation.Ad.Id);
            if (pictures.Any())
            {
                dtoReservation.picture = pictures.ElementAt(0).Path;
            }
        }

        return dtoReservations;
    }
}
using Application.Services;
using Application.UseCases.Reservations.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Reservations;

public class UseCaseFetchMyReservations : IUseCaseParameterizedQuery<IEnumerable<DtoOutputReservation>, int>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationService _reservationService;

    public UseCaseFetchMyReservations(IReservationRepository reservationRepository,
        IReservationService reservationService)
    {
        _reservationRepository = reservationRepository;
        _reservationService = reservationService;
    }

    public IEnumerable<DtoOutputReservation> Execute(int renterId)
    {
        var dbReservations = _reservationRepository.FilterByRenterId(renterId);

        return Mapper.GetInstance().Map<IEnumerable<DtoOutputReservation>>(
            dbReservations.Select(_reservationService.MapToReservation)
        );
    }
}
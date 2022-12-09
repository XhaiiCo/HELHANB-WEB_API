using Application.Services.Reservation;
using Application.UseCases.Reservations.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Reservations;

public class UseCaseRemoveReservation : IUseCaseParameterizedQuery<DtoOutputReservation, DtoInputRemoveReservation>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationService _reservationService;

    public UseCaseRemoveReservation(IReservationRepository reservationRepository,
        IReservationService reservationService)
    {
        _reservationRepository = reservationRepository;
        _reservationService = reservationService;
    }

    public DtoOutputReservation Execute(DtoInputRemoveReservation dto)
    {
        var reservation = _reservationRepository.FindById(dto.reservationId);
        if (reservation.RenterId != dto.userId)
        {
            throw new Exception("Vous n'êtes pas autorisé à supprimer cette annonce");
        }

        var dtoOuputReservation = Mapper.GetInstance()
            .Map<DtoOutputReservation>(_reservationService.MapToReservation(reservation));
        dtoOuputReservation.ArrivalDate = reservation.ArrivalDate;
        dtoOuputReservation.LeaveDate = reservation.LeaveDate;

        if (dtoOuputReservation.ReservationStatus.StatusName == "en attente")
        {
            _reservationRepository.Delete(reservation);
            return dtoOuputReservation;
        }

        throw new Exception("Seulement les réservations en attente peuvent être supprimées");
    }
}
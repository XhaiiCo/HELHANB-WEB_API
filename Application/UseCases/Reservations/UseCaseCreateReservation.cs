using Application.UseCases.Reservations.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Reservations;

public class UseCaseCreateReservation: IUseCaseWriter<DtoOutputReservation, DtoInputCreateReservation>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReservationStatusRepository _reservationStatusRepository;
    
    public UseCaseCreateReservation(IReservationRepository reservationRepository, IUserRepository userRepository, IReservationStatusRepository reservationStatusRepository)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _reservationStatusRepository = reservationStatusRepository;
    }

    public DtoOutputReservation Execute(DtoInputCreateReservation input)
    {
        var mapper = Mapper.GetInstance();
        var dbReservation = mapper.Map<DbReservation>(input);
        dbReservation.ReservationStatusId = 1;
        dbReservation.Creation = DateTime.Now;
        
        _reservationRepository.Create(dbReservation);
        
        var dto = mapper.Map<DtoOutputReservation>(dbReservation);

        var dbRenter = _userRepository.FetchById(dbReservation.RenterId);
        dto.Renter = mapper.Map<DtoOutputReservation.DtoRenter>(dbRenter);
        var dbReservationStatus = _reservationStatusRepository.FetchById(dbReservation.ReservationStatusId);
        dto.Status = mapper.Map<DtoOutputReservation.DtoReservationStatus>(dbReservationStatus);

        return dto;
    }
}
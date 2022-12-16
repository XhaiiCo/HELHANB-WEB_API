using Application.Services.Reservation;
using Domain;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.Services;

public class ReservationService : IReservationService
{
    private readonly IUserRepository _userRepository;

    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationStatusRepository _reservationStatusRepository;
    private readonly IAdRepository _adRepository;

    public ReservationService(
        IUserRepository userRepository,
        IReservationRepository reservationRepository,
        IReservationStatusRepository reservationStatusRepository,
        IAdRepository adRepository
    )
    {
        _userRepository = userRepository;
        _reservationRepository = reservationRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _adRepository = adRepository;
    }

    public Domain.Reservation MapToReservation(DbReservation dbReservation)
    {
        var mapper = Mapper.GetInstance();

        var result = mapper.Map<Domain.Reservation>(dbReservation);

        //Add the renter
        var dbRenter = _userRepository.FetchById(dbReservation.RenterId);
        result.Renter = mapper.Map<Domain.User>(dbRenter);

        //Add the reservation status
        var dbReservationStatus = _reservationStatusRepository.FetchById(dbReservation.ReservationStatusId);
        result.ReservationStatus = mapper.Map<ReservationStatus>(dbReservationStatus);

        //Add the ad
        result.Ad = mapper.Map<Domain.Ad>(_adRepository.FetchById(dbReservation.AdId));

        //Add the dateTimeRange
        result.DateTimeRange = new DateTimeRange
        {
            ArrivalDate = dbReservation.ArrivalDate,
            LeaveDate = dbReservation.LeaveDate,
        };

        return result;
    }

    public DbReservation ConfirmReservation(DbReservation dbReservation)
    {
        dbReservation.ReservationStatusId = 3;
        return _reservationRepository.Update(dbReservation);
    }

    public DbReservation RefuseReservation(DbReservation dbReservation)
    {
        dbReservation.ReservationStatusId = 2;
        return _reservationRepository.Update(dbReservation);
    }
}
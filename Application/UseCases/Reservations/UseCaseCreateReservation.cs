using Application.Services.ReservationBook;
using Application.UseCases.Reservations.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Reservations;

public class UseCaseCreateReservation: IUseCaseWriter<DtoOutputReservation, DtoInputCreateReservation>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReservationStatusRepository _reservationStatusRepository;
    private readonly IAdRepository _adRepository;
    private readonly IReservationBookService _reservationBookService;
    public UseCaseCreateReservation(IReservationRepository reservationRepository, IUserRepository userRepository, IReservationStatusRepository reservationStatusRepository, IAdRepository adRepository, IReservationBookService reservationBookService)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _adRepository = adRepository;
        _reservationBookService = reservationBookService;
    }

    public DtoOutputReservation Execute(DtoInputCreateReservation input)
    {
        var mapper = Mapper.GetInstance();
        var dbReservation = mapper.Map<DbReservation>(input);
        dbReservation.ReservationStatusId = 1;
        dbReservation.Creation = DateTime.Now;
        
        //check if the new reservation is available
        var newDomainReservation = mapper.Map<Reservation>(dbReservation);
        newDomainReservation.dateTimeRange = new DateTimeRange
        {
            _arrivalDate = dbReservation.ArrivalDate,
            _leaveDate = dbReservation.LeaveDate
        };
        
        var reservationBook = _reservationBookService.Fetch(input.AdId);
        
        //on ne garde que celles qui ont le status accepté
        var reservations = (reservationBook.Where(r => r.reservationStatus.Id == 3)).Entries();
        
        if (!Reservation.IsDateAvailable(reservations, newDomainReservation))
            throw new Exception("This date range isn't available");
        
        //si tout s est bien passé
        _reservationRepository.Create(dbReservation);
        
        var dto = mapper.Map<DtoOutputReservation>(dbReservation);

        var dbRenter = _userRepository.FetchById(dbReservation.RenterId);
        dto.Renter = mapper.Map<DtoOutputReservation.DtoRenter>(dbRenter);
        
        var dbReservationStatus = _reservationStatusRepository.FetchById(dbReservation.ReservationStatusId);
        dto.Status = mapper.Map<DtoOutputReservation.DtoReservationStatus>(dbReservationStatus);

        var dbAd = _adRepository.FetchById(dbReservation.AdId);
        dto.Ad = mapper.Map<DtoOutputReservation.DtoAd>(dbAd);     
        
        return dto;
    }
}
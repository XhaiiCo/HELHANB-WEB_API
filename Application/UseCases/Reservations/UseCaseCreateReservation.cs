using Application.Services;
using Application.Services.Date;
using Application.Services.Reservation;
using Application.Services.ReservationBook;
using Application.UseCases.Ads;
using Application.UseCases.Reservations.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Reservations;

public class UseCaseCreateReservation : IUseCaseWriter<DtoOutputReservation, DtoInputCreateReservation>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IAdRepository _adRepository;
    private readonly IReservationBookService _reservationBookService;
    private readonly IDateService _dateService;
    private readonly IReservationService _reservationService;

    public UseCaseCreateReservation(
        IReservationRepository reservationRepository,
        IAdRepository adRepository,
        IReservationBookService reservationBookService,
        IDateService dateService,
        IReservationService reservationService
    )
    {
        _reservationRepository = reservationRepository;
        _adRepository = adRepository;
        _reservationBookService = reservationBookService;
        _dateService = dateService;
        _reservationService = reservationService;
    }


    public DtoOutputReservation Execute(DtoInputCreateReservation input)
    {
        var mapper = Mapper.GetInstance();

        var dbReservation = mapper.Map<DbReservation>(input);

        //Add default params
        dbReservation.ReservationStatusId = 1;
        dbReservation.Creation = DateTime.Now;

        //Change the dateOnly to dateTime with the arrival and leave time 
        var dbAd = _adRepository.FetchById(dbReservation.AdId);
        dbReservation.ArrivalDate = _dateService.DateAndTimeCombiner(_dateService.MapToDateOnly(input.ArrivalDate),
            dbAd.ArrivalTimeRangeStart);
        dbReservation.LeaveDate =
            _dateService.DateAndTimeCombiner(_dateService.MapToDateOnly(input.LeaveDate), dbAd.LeaveTime);

        //check if the new reservation is available
        var newDomainReservation = mapper.Map<Reservation>(dbReservation);
        newDomainReservation.DateTimeRange = new DateTimeRange
        (
            dbReservation.ArrivalDate,
            dbReservation.LeaveDate
        );

        Reservation.ValidNewReservation(newDomainReservation);

        var reservationBook = _reservationBookService.FetchByAdId(input.AdId);

        //Keep only the accepted reservations
        var reservations = (reservationBook.Where(r => r.ReservationStatus.Id == 3)).Entries();

        if (!Reservation.IsDateAvailable(reservations, newDomainReservation))
            throw new Exception("Ces dates sont indisponibles");

        //If all the tests are validated
        _reservationRepository.Create(dbReservation);

        var dto = Mapper.GetInstance().Map<DtoOutputReservation>(_reservationService.MapToReservation(dbReservation));
        return dto;
    }
}
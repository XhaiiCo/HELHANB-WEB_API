﻿using Application.Services.Date;
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
    private readonly IUserRepository _userRepository;
    private readonly IReservationStatusRepository _reservationStatusRepository;
    private readonly IAdRepository _adRepository;
    private readonly IReservationBookService _reservationBookService;
    private readonly IDateService _dateService;

    public UseCaseCreateReservation(IReservationRepository reservationRepository, IUserRepository userRepository,
        IReservationStatusRepository reservationStatusRepository, IAdRepository adRepository,
        IReservationBookService reservationBookService, IDateService dateService)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _adRepository = adRepository;
        _reservationBookService = reservationBookService;
        _dateService = dateService;
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
        var reservations = (reservationBook.Where(r => r.reservationStatus.Id == 3)).Entries();

        if (!Reservation.IsDateAvailable(reservations, newDomainReservation))
            throw new Exception("Ces dates sont indisponibles");

        //If all the tests are validated
        _reservationRepository.Create(dbReservation);

        //Prepare the return
        var dto = mapper.Map<DtoOutputReservation>(dbReservation);

        //Add the renter
        var dbRenter = _userRepository.FetchById(dbReservation.RenterId);
        dto.Renter = mapper.Map<DtoOutputReservation.DtoRenter>(dbRenter);

        //Add the reservation status
        var dbReservationStatus = _reservationStatusRepository.FetchById(dbReservation.ReservationStatusId);
        dto.Status = mapper.Map<DtoOutputReservation.DtoReservationStatus>(dbReservationStatus);

        //Add the ad
        dto.Ad = mapper.Map<DtoOutputReservation.DtoAd>(dbAd);

        return dto;
    }
}
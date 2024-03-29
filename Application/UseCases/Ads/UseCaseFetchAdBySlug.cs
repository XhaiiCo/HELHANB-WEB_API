﻿using Application.Services.Ad;
using Application.Services.ReservationBook;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Ads;

public class UseCaseFetchAdBySlug : IUseCaseParameterizedQuery<DtoOutputAdWithReservations, string>
{
    private readonly IAdService _adService;
    private readonly IReservationRepository _reservationRepository;

    public UseCaseFetchAdBySlug(IAdService adService, IReservationRepository reservationRepository)
    {
        _adService = adService;
        _reservationRepository = reservationRepository;
    }

    public DtoOutputAdWithReservations Execute(string slug)
    {
        var ad = _adService.FetchBySlug(slug);

        if (ad.Status.StatusName is not ("acceptée" or "désactivée"))
            throw new Exception("Annonce indisponible");

        var dto = Mapper.GetInstance().Map<DtoOutputAdWithReservations>(ad);

        //Only the accepted reservation
        var reservations = _reservationRepository.FilterByAdId(ad.Id).Where(item => item.ReservationStatusId == 3);

        var reservationsList = reservations.Select(reservation =>
            new DtoOutputAdWithReservations.DtoOutputAdReservation
            {
                ArrivalDate = reservation.ArrivalDate,
                LeaveDate = reservation.LeaveDate
            }).ToList();

        dto.Reservations = reservationsList;

        return dto;
    }
}
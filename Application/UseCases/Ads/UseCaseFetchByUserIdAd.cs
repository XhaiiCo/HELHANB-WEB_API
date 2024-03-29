﻿using Application.Services.Ad;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.Reservation;

namespace Application.UseCases.Ads;

public class UseCaseFetchByUserIdAd : IUseCaseParameterizedQuery<IEnumerable<DtoOutputMyAdsAd>, int>
{
    private readonly IAdService _adService;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReservationStatusRepository _reservationStatusRepository;

    public UseCaseFetchByUserIdAd(IAdService adService, IReservationRepository reservationRepository, IUserRepository userRepository, IReservationStatusRepository reservationStatusRepository)
    {
        _adService = adService;
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _reservationStatusRepository = reservationStatusRepository;
    }

    public IEnumerable<DtoOutputMyAdsAd> Execute(int id)
    {
        var ads = _adService.FetchByUserId(id).ToArray();

        var dtos = Mapper.GetInstance().Map<IEnumerable<DtoOutputMyAdsAd>>(ads).ToArray();
        
        for (int i = 0; i < dtos.Length; i++)
        {
            var reservations = _reservationRepository.FilterByAdId(ads[i].Id);
            
            var reservationsList = reservations.Select(reservation =>
                new DtoOutputMyAdsAd.DtoOutputAdReservationMyAds
                {
                    Id = reservation.Id,
                    ArrivalDate = reservation.ArrivalDate,
                    LeaveDate = reservation.LeaveDate,
                    Creation = reservation.Creation,
                    RenterMyAds = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoRenterMyAds>(_userRepository.FetchById(reservation.RenterId)),
                    StatusMyAds = Mapper.GetInstance().Map<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoReservationStatusMyAds>(_reservationStatusRepository.FetchById(reservation.ReservationStatusId)),
                }).ToList();

            dtos[i].Reservations = reservationsList;
        }

        return dtos;
    }
}
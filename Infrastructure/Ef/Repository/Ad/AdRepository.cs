﻿using System.Runtime.CompilerServices;
using Domain;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Reservation;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Ef.Repository.Ad;

public class AdRepository : IAdRepository
{
    private readonly HelhanbContextProvider _contextProvider;
    private readonly IReservationRepository _reservationRepository;

    public AdRepository(HelhanbContextProvider contextProvider, IReservationRepository reservationRepository)
    {
        _contextProvider = contextProvider;
        _reservationRepository = reservationRepository;
    }

    public IEnumerable<DbAd> FetchAll(FilteringAd filter)
    {
        using var context = _contextProvider.NewContext();

        return context.Ads.Where(ad => filter.StatusId == null || ad.AdStatusId == filter.StatusId).ToList();
    }

    public IEnumerable<string> FetchDistinctsCountries()
    {
        using var context = _contextProvider.NewContext();

        return context.Ads.Select(item => item.Country).Distinct().ToList();
    }

    public IEnumerable<string> FetchDistinctsCitiesByCountry(string country)
    {
        using var context = _contextProvider.NewContext();

        return context.Ads.Where(item => item.Country == country).Select(item => item.City).Distinct().ToList();
    }

    public IEnumerable<DbAd> FetchRange(FilteringAd filter)
    {
        using var context = _contextProvider.NewContext();
        
        return context.Ads.Where(ad => (filter.StatusId == null || ad.AdStatusId == filter.StatusId)
                                && (filter.Country == null || ad.Country == filter.Country)
                                && (filter.City == null || ad.City == filter.City)
                                && (filter.PricePerNight == null || ad.PricePerNight <= filter.PricePerNight)
                                && (filter.NumberOfPersons == null || ad.NumberOfPersons >= filter.NumberOfPersons)
                                ).Skip(filter.Offset).Take(filter.Limit).ToList();
    }
    
    public IEnumerable<DbAd> FetchByUserId(int id)
    {
        using var context = _contextProvider.NewContext();

        return context.Ads.Where(ad => ad.UserId == id).ToList();
    }

    public DbAd Create(DbAd ad)
    {
        using var context = _contextProvider.NewContext();

        context.Ads.Add(ad);
        context.SaveChanges();
        return ad;
    }

    public DbAd FetchById(int id)
    {
        using var context = _contextProvider.NewContext();
        var ad = context.Ads.FirstOrDefault(ad => ad.Id == id);

        if (ad == null)
            throw new KeyNotFoundException($"Ad with id {id} has not been found");

        return ad;
    }
    
    public DbAd FetchBySlug(string slug)
    {
        using var context = _contextProvider.NewContext();
        var ad = context.Ads.FirstOrDefault(ad => ad.AdSlug == slug);

        if (ad == null)
            throw new KeyNotFoundException($"Ad with slug {slug} has not been found");

        return ad;
    }

    public DbAd Delete(DbAd ad)
    {
        using var context = _contextProvider.NewContext();

        context.Ads.Remove(ad);
        context.SaveChanges();

        return ad;
    }

    public DbAd Update(DbAd ad)
    {
        using var context = _contextProvider.NewContext();

        context.Attach(ad);
        context.Entry(ad).State = EntityState.Modified;
        context.SaveChanges();

        return ad;
    }

    /*public DbAd FetchByCountry(string country)
    {
        using var context = _contextProvider.NewContext();
        var ad = context.Ads.FirstOrDefault(a => a.Country == country);

        //if (ad == null)
        
        
    }*/
    
    public int Count(FilteringAd filter)
    {
        using var context = _contextProvider.NewContext();
        
        var dbAds = context.Ads.Where(ad => (filter.StatusId == null || ad.AdStatusId == filter.StatusId)
                                       && (filter.Country == null || ad.Country == filter.Country)
                                       && (filter.City == null || ad.City == filter.City)
                                       && (filter.PricePerNight == null || ad.PricePerNight <= filter.PricePerNight)
                                       && (filter.NumberOfPersons == null || ad.NumberOfPersons >= filter.NumberOfPersons)).ToList();


        if (filter.ArrivalDate == null || filter.LeaveDate == null) return dbAds.Count;
        
        var filterReservation = new Domain.Reservation
        {
            DateTimeRange = new DateTimeRange(DateTime.Parse(filter.ArrivalDate).Date, DateTime.Parse(filter.LeaveDate).Date)
        };

        Domain.Reservation.ValidNewReservation(filterReservation);

        for (var i = dbAds.Count - 1; i >= 0; i--)
        {
            var dbReservations = _reservationRepository.FilterByAdId(dbAds[i].Id).Where(dbReservation => dbReservation.ReservationStatusId == 3);

            var reservations = dbReservations.Select(dbReservation =>  new Domain.Reservation
            {
                DateTimeRange = new DateTimeRange(dbReservation.ArrivalDate, dbReservation.LeaveDate)
            });

            if(!Domain.Reservation.IsDateAvailable(reservations, filterReservation))
            {
                dbAds.RemoveAt(i);
            }
        }

        return dbAds.Count;
    }
}
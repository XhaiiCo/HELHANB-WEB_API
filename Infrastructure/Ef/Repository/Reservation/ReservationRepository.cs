﻿using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository.Reservation;

public class ReservationRepository: IReservationRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public ReservationRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }
    
    public DbReservation Create(DbReservation reservation)
    {
        using var context = _contextProvider.NewContext();
        context.Reservations.Add(reservation);
        context.SaveChanges();
        return reservation;
    }

    public IEnumerable<DbReservation> FilterByAdId(int adId)
    {
        using var context = _contextProvider.NewContext();
        var reservations = context.Reservations
            .Where(dbR => dbR.AdId == adId)
            .ToList();
        
        return reservations;
    }
}
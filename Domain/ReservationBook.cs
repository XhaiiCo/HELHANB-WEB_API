﻿namespace Domain;

public class ReservationBook
{
    private readonly List<Reservation> _entries = new();

    public ReservationBook Where(Predicate<Reservation> predicate)
    {
        return ReservationBook.Of(_entries.Where(predicate.Invoke));
    }
    
    public static ReservationBook Of(IEnumerable<Reservation> reservations)
    {
        var reservationBook = new ReservationBook();
        reservationBook.AddRange(reservations);
        return reservationBook;
    }

    public void Add(Reservation reservation)
    {
        _entries.Add(reservation);
    }

    public void AddRange(IEnumerable<Reservation> reservations)
    {
        foreach (var reservation in reservations)
            Add(reservation);
    }
    
    public IEnumerable<Reservation> Entries()
    {
        return _entries;
    }
/*
    private List<Reservation> _Reservations;
    public Ad Ad { get; set; }

    public List<Reservation> Reservations
    {
        get => _Reservations;
        set { value.ForEach(reservation => AddReservation(reservation)); }
    }

    public bool AddReservation(Reservation reservation)
    {
        if (!Reservation.IsDateAvailable(Reservations, reservation)) return false;

        Reservations.Add(reservation);
        return true;
    }*/
}
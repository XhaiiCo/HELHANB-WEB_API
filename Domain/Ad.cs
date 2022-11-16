using System.Diagnostics;

namespace Domain;

public class Ad
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public float PricePerNight { get; set; }
    public string Description { get; set; }
    public int NumberOfPersons { get; set; }
    public int NumberOfBedrooms { get; set; }
    public string Street { get; set; }
    public int PostalCode { get; set; }
    public string Country { get; set; }
    public string City { get; set; }

    public List<Picture> Pictures;
    private List<Reservation> _Reservations;

    public List<Reservation> Reservations
    {
        get => _Reservations;
        set { value.ForEach(reservation => AddReservation(reservation)); }
    }

    public bool AddReservation(Reservation reservation)
    {
        if (!IsDateAvailable(reservation.ArrivalDate, reservation.LeaveDate)) return false;
        
        this.Reservations.Add(reservation);
        return true;

    }

    private bool IsDateAvailable(DateOnly dateArrival, DateOnly dateLeave)
    {
        foreach (var reservation in this.Reservations)
        {
            //TODO: speak about it 

            //If the booking starts after
            if (reservation.ArrivalDate > dateLeave) continue;

            //If the booking ends before
            if (reservation.LeaveDate < dateArrival) continue;

            return false;
        }
        
        return true;
    }
}
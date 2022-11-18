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
    public int  AdStatusId { get; set; } 

    public List<Picture> Pictures;
    private List<Reservation> _Reservations;
    
    public Ad()
    {
        Pictures = new List<Picture>();
        _Reservations = new List<Reservation>();
    }
    public List<Reservation> Reservations
    {
        get => _Reservations;
        set { value.ForEach(reservation => AddReservation(reservation)); }
    }

    public bool AddReservation(Reservation reservation)
    {
        if (!Reservation.IsDateAvailable(Reservations, reservation)) return false;
        
        this.Reservations.Add(reservation);
        return true;

    }
}
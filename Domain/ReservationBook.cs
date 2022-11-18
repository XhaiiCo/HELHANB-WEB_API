namespace Domain;

public class ReservationBook
{
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
    }
}
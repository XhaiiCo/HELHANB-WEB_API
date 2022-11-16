namespace Domain;

public class Reservation
{
    public int Id { get; set; }
    public DateTime Creation { get; set; }
    public DateOnly ArrivalDate { get; set; }
    public DateOnly LeaveDate { get; set; }

    public ReservationStatus ReservationStatus { get; set; }
    
    public static bool IsDateAvailable(List<Reservation> reservations, Reservation newReservation)
    {
        foreach (var reservation in reservations)
        {
            //TODO: speak about it 

            //If the booking starts after
            if (reservation.ArrivalDate > newReservation.LeaveDate) continue;

            //If the booking ends before
            if (reservation.LeaveDate < newReservation.ArrivalDate) continue;

            return false;
        }
        
        return true;
    }
}
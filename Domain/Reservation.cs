namespace Domain;

public class Reservation
{
    public int Id { get; set; }
    public DateTime Creation { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime LeaveDate { get; set; }

    public ReservationStatus ReservationStatus { get; set; }
    
    public static bool IsDateAvailable(List<Reservation> reservations, Reservation newReservation)
    {
        foreach (var reservation in reservations)
        {
            //TODO: speak about it 

            //If the booking starts after
            if (newReservation.LeaveDate < reservation.ArrivalDate) continue;

            //If the booking ends before
            if (newReservation.ArrivalDate > reservation.LeaveDate) continue;

            return false;
        }
        
        return true;
    }
}
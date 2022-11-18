namespace Domain;

public class Reservation
{
    public int Id { get; set; }
    public DateTime Creation { get; set; }
    public DateTimeRange dateTimeRange { get; set; }
    
    public ReservationStatus reservationStatus { get; set; }
    
    
    
    public static bool IsDateAvailable(IEnumerable<Reservation> reservations, Reservation newReservation)
    {
        foreach (var reservation in reservations)
        {
            //TODO: speak about it 

            //If the booking starts after
            if (newReservation.dateTimeRange.LeaveDate < reservation.dateTimeRange.ArrivalDate) continue;

            //If the booking ends before
            if (newReservation.dateTimeRange.ArrivalDate > reservation.dateTimeRange.LeaveDate) continue;

            return false;
        }
        
        return true;
    }
}
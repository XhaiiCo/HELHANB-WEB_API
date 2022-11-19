namespace Domain;

public class Reservation
{
    public int Id { get; set; }
    public DateTime Creation { get; set; }

    public DateTimeRange DateTimeRange { get; set; }
    
    public ReservationStatus reservationStatus { get; set; }
    
    public User Renter { get; set; }
    
    public static bool IsDateAvailable(IEnumerable<Reservation> reservations, Reservation newReservation)
    {
        foreach (var reservation in reservations)
        {
            //If the booking starts after
            if (newReservation.DateTimeRange.LeaveDate < reservation.DateTimeRange.ArrivalDate) continue;

            //If the booking ends before
            if (newReservation.DateTimeRange.ArrivalDate > reservation.DateTimeRange.LeaveDate) continue;

            return false;
        }

        return true;
    }

    public int ComputeNbNight()
    {
        throw new NotImplementedException();
    }

    public double ComputeReservationPrice(double priceByNight)
    {
        return this.ComputeNbNight() * priceByNight;
    }
}
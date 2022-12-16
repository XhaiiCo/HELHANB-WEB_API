namespace Domain;

public class Reservation
{
    public int Id { get; set; }
    public DateTime Creation { get; set; }

    public DateTimeRange DateTimeRange;

    public ReservationStatus ReservationStatus { get; set; }
    public Ad Ad { get; set; }

    public User Renter { get; set; }

    /// <summary>
    /// Check that the new reservation does not conflict with the one in the list
    /// </summary>
    /// <param name="reservations">The list of reservations that are already in the system.</param>
    /// <param name="newReservation">The reservation that is being checked for availability.</param>
    /// <returns>
    /// true if the dates of the new reservation is available
    /// </returns>
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

    /// <summary>
    /// A reservation must be for at least the following day, at least one night and the arrival date cannot be in the past
    /// </summary>
    /// <param name="reservation">The reservation object that we want to validate</param>
    /// <returns>
    /// A boolean
    /// </returns>
    public static bool ValidNewReservation(Reservation reservation)
    {
        //At least 1 night
        //The arrival date cannot be in the past
        if (IsInThePast(reservation))
            throw new Exception("La date d'arrivée ne peut pas être passée");

        //A reservation must be for at least the following day
        if (IsForTheSameDay(reservation))
            throw new Exception("Une réservation doit être au minimum pour le lendemain");

        //The reservation must be for at least one night
        if (IsLessThanOneNight(reservation))
            throw new Exception("La réservation doit au moins faire une nuit");

        return true;
    }

    /// <summary>
    /// This function checks if the reservation is not bigger than one night
    /// </summary>
    /// <param name="reservation">the reservation object that is being validated</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public static bool IsLessThanOneNight(Reservation reservation)
    {
        return reservation.DateTimeRange.ComputeNbNight() == 0;
    }

    /// <summary>
    /// If the arrival date is today, then return true
    /// </summary>
    /// <param name="reservation">The reservation object that is being validated.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public static bool IsForTheSameDay(Reservation reservation)
    {
        return reservation.DateTimeRange.ArrivalDate.Day == DateTime.Now.Day &&
               reservation.DateTimeRange.ArrivalDate.Month == DateTime.Now.Month &&
               reservation.DateTimeRange.ArrivalDate.Year == DateTime.Now.Year;
    }

    /// <summary>
    /// If the arrival date is less than the current date, then the reservation is in the past
    /// </summary>
    /// <param name="reservation">The reservation object that is being validated.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public static bool IsInThePast(Reservation reservation)
    {
        return reservation.DateTimeRange.ArrivalDate.CompareTo(DateTime.Now) < 0;
    }


    /// <summary>
    /// Compute the price of a reservation by multiplying the number of nights by the price by night.
    /// </summary>
    /// <param name="priceByNight">the price per night of the room</param>
    /// <returns>
    /// The number of nights in the reservation multiplied by the price per night.
    /// </returns>
    public double ComputeReservationPrice(double priceByNight)
    {
        return DateTimeRange.ComputeNbNight() * priceByNight;
    }
}
namespace Domain;

public class DateTimeRange
{
    private DateTime _arrivalDate;
    private DateTime _leaveDate;

    public DateTimeRange(){}

    public DateTimeRange(DateTime arrivalDate, DateTime leaveDate)
    {
        ArrivalDate = arrivalDate;
        LeaveDate = leaveDate;
    }

    public DateTime ArrivalDate
    {
        get => _arrivalDate;
        set
        {
            //If the date is already initialized
            if (!_leaveDate.Equals(new DateTime(1, 1, 1)))
                if (value.CompareTo(_leaveDate) >= 0)
                    throw new ArgumentException($"La date d'arrivée ne peut pas être après la date de départ");

            _arrivalDate = value;
        }
    }

    public DateTime LeaveDate
    {
        get => _leaveDate;
        set
        {
            //If the date is already initialized
            if (!_arrivalDate.Equals(new DateTime(1, 1, 1)))
                if (value.CompareTo(_arrivalDate) <= 0)
                    throw new ArgumentException($"La date de départ ne peut pas être avant la date d'arrivée");

            _leaveDate = value;
        }
    }
    
    /// <summary>
    /// Compute the number of nights between the arrival and leave dates
    /// </summary>
    /// <returns>
    /// The number of nights between the arrival and leave dates.
    /// </returns>
    public int ComputeNbNight()
    {
        decimal totalDays = LeaveDate.Day - ArrivalDate.Day;
        return (int) Math.Floor(totalDays);
    }
}
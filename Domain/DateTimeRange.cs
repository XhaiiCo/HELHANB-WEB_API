namespace Domain;

public class DateTimeRange
{
    private DateTime _arrivalDate;
    private DateTime _leaveDate;

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
                    throw new ArgumentException(
                        $"Arrival date can't be after leaving date");

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
                    throw new ArgumentException($"Leaving date can't be before arrival date");

            _leaveDate = value;
        }
    }
}
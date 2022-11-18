namespace Domain;

public class DateTimeRange
{
    private DateTime _arrivalDate;
    private DateTime _leaveDate;

    public DateTimeRange(DateTime arrivalDate, DateTime leaveDate)
    {
        this.ArrivalDate = arrivalDate;
        this.LeaveDate = leaveDate;
    } 

    public DateTime ArrivalDate
    {
        get => _arrivalDate;
        set
        {
            if(value >= LeaveDate)
                throw new ArgumentException($"Arrival date can't be after leaving date");

            _arrivalDate = value;
        }
    }

    public DateTime LeaveDate
    {
        get => _leaveDate;
        set
        {
            if(value <= ArrivalDate)
                throw new ArgumentException($"Leaving date can't be before arrival date");

            _leaveDate = value;
        }
    }

}
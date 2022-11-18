namespace Domain;

public class DateTimeRange
{
    public DateTime _arrivalDate;
    public DateTime _leaveDate;

    public DateTime ArrivalDate
    {
        get => _arrivalDate;
        set
        {
            /*
            if(value >= LeaveDate)
                throw new ArgumentException($"Arrival date can't be after leaving date");*/

            _arrivalDate = value;
        }
    }

    public DateTime LeaveDate
    {
        get => _leaveDate;
        set
        {/*
            if(value <= ArrivalDate)
                throw new ArgumentException($"Leaving date can't be before arrival date");*/

            _leaveDate = value;
        }
    }

}
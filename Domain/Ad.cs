namespace Domain;

public class Ad
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public float PricePerNight { get; set; }
    public string Description { get; set; }
    public int NumberOfPersons { get; set; }
    public int NumberOfBedrooms { get; set; }
    public string Street { get; set; }
    public int PostalCode { get; set; }
    public string Country { get; set; }
    public string City { get; set; }

    public TimeSpan ArrivalTimeRangeStart { get; set; }
    public TimeSpan ArrivalTimeRangeEnd { get; set; }
    public TimeSpan LeaveTime { get; set; }

    public string AdSlug { get; set; }
    public User Owner { get; set; }

    public AdStatus Status { get; set; }
    private List<string> _features;

    public List<string> Features
    {
        get => _features;
        set { value.ForEach(feature => AddFeature(feature)); }
    }

    private List<Picture> _pictures;

    public List<Picture> Pictures
    {
        get => _pictures;
        set { value.ForEach(picture => AddPicture(picture)); }
    }

    public Ad()
    {
        _pictures = new List<Picture>();
        _features = new List<string>();
    }

    public bool AddPicture(Picture picture)
    {
        if (_pictures.Count >= 15) return false;

        foreach (var p in _pictures)
        {
            if (p.Equals(picture)) return false;
        }

        _pictures.Add(picture);
        return true;
    }

    public bool AddFeature(string feature)
    {
        if (_features.Contains(feature)) return false;

        _features.Add(feature);
        return true;
    }

    /// <summary>
    /// The function checks if the arrival time is before the departure time, and if the arrival end time is after the
    /// arrival start time
    /// </summary>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public static bool ValidHours(TimeSpan arrivalStart, TimeSpan arrivalEnd, TimeSpan leave)
    {
        if (!IsHour2IsAfterHour1(leave, arrivalStart))
            throw new Exception("L'heure de départ doit être avant l'heure d'arrivée");

        if (!IsHour2IsAfterHour1(arrivalStart, arrivalEnd))
            throw new Exception("Heures d'arrivée incorrectes");

        return true;
    }

    /// <summary>
    /// Check if hour 2 is after hour 1 
    /// </summary>
    /// <returns>
    /// True or False
    /// </returns>
    public static bool IsHour2IsAfterHour1(TimeSpan hour1, TimeSpan hour2)
    {
        return hour1 < hour2;
    }
}
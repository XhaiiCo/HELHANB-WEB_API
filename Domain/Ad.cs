using System.Diagnostics;
using System.Security.Cryptography;

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
    public int AdStatusId { get; set; }
    
    public TimeSpan ArrivalTimeRangeStart { get; set; }
    public TimeSpan ArrivalTimeRangeEnd { get; set; }
    public TimeSpan LeaveTime { get; set; }
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
}
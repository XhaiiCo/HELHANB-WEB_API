namespace Application.UseCases.Ads.Dtos;

public class DtoOutputAdWithReservations
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

    public IEnumerable<string> Features { get; set; }
    public IEnumerable<DtoOutputAdPicture> Pictures { get; set; }
    public DtoOutputUserInAd Owner { get; set; }
    public IEnumerable<DtoOutputAdReservation> Reservations { get; set; }

    public class DtoOutputAdReservation
    {
        public DateTime ArrivalDate { get; set; }

        public DateTime LeaveDate { get; set; }
    }
}
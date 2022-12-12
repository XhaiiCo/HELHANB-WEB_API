namespace Application.UseCases.Ads.Dtos;

public class DtoOutputMyAdsAd
{
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

    public string AdSlug { get; set; }

    public TimeSpan ArrivalTimeRangeStart { get; set; }
    public TimeSpan ArrivalTimeRangeEnd { get; set; }
    public TimeSpan LeaveTime { get; set; }

    public IEnumerable<string> Features { get; set; }
    public IEnumerable<DtoOutputAdPicture> Pictures { get; set; }
    public DtoOutputUserInAd Owner { get; set; }
    public DtoOutputAdStatus status { get; set; }
    public IEnumerable<DtoOutputAdReservationMyAds> Reservations { get; set; }

    public class DtoOutputAdReservationMyAds
    { 
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime LeaveDate { get; set; }

        public DtoReservationStatusMyAds StatusMyAds { get; set; }
        public DtoRenterMyAds RenterMyAds { get; set; }
        public DateTime Creation { get; set; }

        public class DtoRenterMyAds
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string? ProfilePicturePath { get; set; }
        }

        public class DtoReservationStatusMyAds
        {
            public int Id { get; set; }
            public string StatusName { get; set; }
        }
    }
}
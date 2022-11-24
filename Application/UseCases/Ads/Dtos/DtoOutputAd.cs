using Application.UseCases.Roles.Dtos;
using Application.UseCases.Users.Dtos;
using Domain;

namespace Application.UseCases.Ads.Dtos;

public class DtoOutputAd
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

    public class DtoOutputUserInAd
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicturePath { get; set; }
    }
}
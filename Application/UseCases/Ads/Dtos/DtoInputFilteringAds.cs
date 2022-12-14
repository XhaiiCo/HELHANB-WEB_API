namespace Application.UseCases.Ads.Dtos;

public class DtoInputFilteringAds
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public int? StatusId { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public float? PricePerNight { get; set; }
    public int? NumberOfPersons { get; set; }
    
    public string? ArrivalDate { get; set; }
    
    public string? LeaveDate { get; set; }
}
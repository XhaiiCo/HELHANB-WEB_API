namespace Application.UseCases.Ads.Dtos;

public class DtoInputFilteringAds
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    
    public int? StatusId { get; set; }
    public string? Name { get; set; }
}
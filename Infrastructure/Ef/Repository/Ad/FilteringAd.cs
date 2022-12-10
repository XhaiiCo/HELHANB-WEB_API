namespace Infrastructure.Ef.Repository.Ad;

public class FilteringAd
{
    public int Limit { get; set; }
    public int Offset { get; set; }
    public int? StatusId { get; set; }
    public string? Country { get; set; }
}
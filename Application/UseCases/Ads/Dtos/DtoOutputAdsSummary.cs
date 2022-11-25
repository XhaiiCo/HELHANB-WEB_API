namespace Application.UseCases.Ads.Dtos;

public class DtoOutputAdsSummary
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string City { get; set; }
    
    public string Country { get; set; }
    
    public int NumberOfBedrooms { get; set; }
    
    public int NumberOfPersons { get; set; }
    
    public float PricePerNight { get; set; }
    
    public IEnumerable<string> Pictures { get; set; }
}

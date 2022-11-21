using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Ads.Dtos;

public class DtoInputUpdateAd
{
    [Required] public string Name { get; set; }
    [Required] public float PricePerNight { get; set; }
    [Required] public string Description { get; set; }
    [Required] public int NumberOfPersons { get; set; }
    [Required] public int NumberOfBedrooms { get; set; }
}
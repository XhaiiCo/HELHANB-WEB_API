using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Ads.Dtos;

public class DtoInputUpdateAd
{
    [Required] public string Name { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    [Required] public float PricePerNight { get; set; }
    [Required] public string Description { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    [Required] public int NumberOfPersons { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    [Required] public int NumberOfBedrooms { get; set; }
}
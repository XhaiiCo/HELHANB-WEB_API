using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Ads.Dtos;

public class DtoInputCreateAd
{
    [Required] public string Name { get; set; }
    [Required] public float PricePerNight { get; set; }
    [Required] public string Description { get; set; }
    [Required] public int NumberOfPersons { get; set; }
    [Required] public int NumberOfBedrooms { get; set; }
    [Required] public string Street { get; set; }
    [Required] public int PostalCode { get; set; }
    [Required] public string Country { get; set; }
    [Required] public string City { get; set; }
    [Required] public int UserId { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Ads.Dtos;

public class DtoInputUpdateAd
{
    [Required] public int id { get; set; }
    
    [Required] public string Name { get; set; }
    
    //[Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    [Required] public int NumberOfPersons { get; set; }
    
    //[Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    [Required] public int NumberOfBedrooms { get; set; }
    
    [Required] public string Description { get; set; }
    
    //[Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    [Required] public float PricePerNight { get; set; }
    
    [Required] public DtoInputTime ArrivalTimeRangeStart { get; set; }
    [Required] public DtoInputTime ArrivalTimeRangeEnd { get; set; }
    [Required] public DtoInputTime LeaveTime { get; set; }

    [Required] public IEnumerable<string> Features { get; set; }

    [Required] public IEnumerable<string> picturesToAdd { get; set; }
    
    [Required] public IEnumerable<string> picturesToDelete { get; set; }

    
    
}
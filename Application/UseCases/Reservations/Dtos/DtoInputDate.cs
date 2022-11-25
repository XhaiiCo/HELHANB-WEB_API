using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Reservations.Dtos;

public class DtoInputDateOnly
{
    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    [Required] public int Year { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    [Required] public int Month { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    [Required] public int Day { get; set; }
}
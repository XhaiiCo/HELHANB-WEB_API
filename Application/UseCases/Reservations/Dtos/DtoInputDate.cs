using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Reservations.Dtos;

public class DtoInputDateOnly
{
    [Required] public int Year { get; set; }
    [Required] public int Month { get; set; }
    [Required] public int Day { get; set; }
}
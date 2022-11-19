using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Reservations.Dtos;

public class DtoInputCreateReservation
{
    [JsonIgnore] public int AdId { get; set; }
    [Required] public int RenterId { get; set; }

    [Required]
    public DtoInputDateOnly ArrivalDate { get; set; }
    
    [Required]
    public DtoInputDateOnly LeaveDateOnly { get; set; }
}
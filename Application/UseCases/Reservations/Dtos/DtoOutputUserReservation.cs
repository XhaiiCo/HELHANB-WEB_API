using System.ComponentModel.DataAnnotations;
using Application.UseCases.Reservations.Dtos;

namespace Application.UseCases.Ads.Dtos;

public class DtoOutputUserReservation
{
    [Required] public int Id { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? ProfilePicturePath { get; set; }
    [Required] public int RenterId { get; set; }
    [Required] public DtoInputDateOnly ArrivalDate { get; set; }
    [Required] public DtoInputDateOnly LeaveDate { get; set; }
    [Required] public DtoInputDateOnly ReservationDate { get; set; }
}
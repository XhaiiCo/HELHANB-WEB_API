namespace Application.UseCases.Reservations.Dtos;

public class DtoOutputMyAdReservation
{
    public int Id { get; set; }
    
    public DateTime ArrivalDate { get; set; }
    
    public DateTime LeaveDate { get; set; }
    
    public DateTime Creation { get; set; }
    
    public DtoOutputReservation.DtoRenter Renter { get; set; }
    
    public DtoOutputReservation.DtoReservationStatus ReservationStatus { get; set; }
}




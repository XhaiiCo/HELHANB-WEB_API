namespace Infrastructure.Ef.DbEntities;

public class DbReservation
{
    public int Id { get; set; }
    
    public DateTime Creation { get; set; }
    
    public DateOnly ArrivalDate { get; set; }
    
    public DateOnly LeaveDate { get; set; }
    
    
    public int ReservationStatusId { get; set; }
    
    public int AdId { get; set; }
    
    public int Renter { get; set; }
}
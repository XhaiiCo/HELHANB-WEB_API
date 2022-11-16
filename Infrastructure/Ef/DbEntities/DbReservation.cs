using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Ef.DbEntities;

public class DbReservation
{
    public int Id { get; set; }
    
    public DateTime Creation { get; set; }
    
    public DateTime ArrivalDate { get; set; }
    
    public DateTime LeaveDate { get; set; }

    public int ReservationStatusId { get; set; }
    
    public int AdId { get; set; }
    
    public int Renter { get; set; }
}
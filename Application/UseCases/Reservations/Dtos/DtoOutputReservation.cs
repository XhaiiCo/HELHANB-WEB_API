namespace Application.UseCases.Reservations.Dtos;

public class DtoOutputReservation
{
    public int Id { get; set; }
    
    public DateTime ArrivalDate { get; set; }
    
    public DateTime LeaveDate { get; set; }
    
    public DtoRenter Renter { get; set; }
    
    public DtoAd Ad { get; set; }
    public DtoReservationStatus ReservationStatus { get; set; }
    
    public class DtoRenter
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    
    public class DtoReservationStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
    }

    public class DtoAd
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}




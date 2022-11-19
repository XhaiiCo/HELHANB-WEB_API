namespace Application.Services.ReservationBook;

public interface IReservationBookService
{
     Domain.ReservationBook FetchByAdId(int adId);
}
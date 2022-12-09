using System.Xml;
using Application.Services.Date;
using Application.Services.ReservationBook;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Users;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Reservation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Reservations;

public class UseCaseFetchAllReservationToConfirm: IUseCaseParameterizedQuery<IEnumerable<DtoOutputUserReservation>, string>
{
    private readonly IReservationBookService _reservationBookService;
    private readonly IUserRepository _userRepository;
    private readonly IDateService _dateService;
    private readonly IReservationRepository _reservationRepository;

    public UseCaseFetchAllReservationToConfirm (IReservationBookService reservationBookService, IUserRepository userRepository, IDateService dateService, IReservationRepository reservationRepository)
    {
        _reservationBookService = reservationBookService;
        _userRepository = userRepository;
        _dateService = dateService;
        _reservationRepository = reservationRepository;
    }

    public IEnumerable<DtoOutputUserReservation> Execute(string adSlug)
    {
        var mapper = Mapper.GetInstance();
        
        var reservationBook = _reservationBookService.FetchReservationToConfirmByAdSlug(adSlug);

        var reservations = reservationBook.Entries();

        var dtoReservations = mapper.Map<IEnumerable<DtoOutputUserReservation>>(reservations);

        foreach (var dtoReservation in dtoReservations)
        {
            var reservation = reservations.FirstOrDefault(r => r.Id == dtoReservation.Id);
            var dbReservation = _reservationRepository.FindById(dtoReservation.Id);
            var dbUser = _userRepository.FetchById(dtoReservation.RenterId);
            
            dtoReservation.ReservationDate = _dateService.MapToDtoInputDateOnly(reservation.Creation);
            dtoReservation.ArrivalDate = _dateService.MapToDtoInputDateOnly(dbReservation.ArrivalDate);
            dtoReservation.LeaveDate = _dateService.MapToDtoInputDateOnly(dbReservation.LeaveDate);
            
            dtoReservation.ProfilePicturePath = dbUser.ProfilePicturePath;
            dtoReservation.FirstName = dbUser.FirstName;
            dtoReservation.LastName = dbUser.LastName;
        }

        return dtoReservations;
        /*
         var test = Mapper.GetInstance().Map<IEnumerable<DtoOutputUserReservation>>(reservations);
         foreach (var reservation in test)
        {
            Console.WriteLine(reservation.Id  + "  " + reservation.FirstName);
        }

        return test;*/
    }
}
using System.Diagnostics;
using Domain;

namespace Tests;

public class AdTests
{
    /*
    [Test]
    [TestCase("01/01/2022 07:00", "01/01/2022 11:00", false, 1)]
    [TestCase("25/12/2021 07:00", "01/01/2022 07:00", true, 2)]
    [TestCase("13/01/2022 21:00", "17/01/2022 22:00", false, 1)]
    [TestCase("13/01/2022 23:00", "17/01/2022 22:00", true, 2)]
    public void AddReservation(string dateArrival, string dateLeave, bool expected, int size)
    {
        Ad ad = new Ad();
        ReservationBook reservationBook = new ReservationBook();

        reservationBook.Add(new Reservation
        {
            DateTimeRange = new DateTimeRange
            (
                new DateTime(2022, 01, 01, 10, 0, 0),
                new DateTime(2022, 01, 13, 22, 0, 0)
            )
        });

        bool testAddReservation = reservationBook.Add(new Reservation
        {
            DateTimeRange = new DateTimeRange
            (
                DateTime.Parse(dateArrival),
                DateTime.Parse(dateLeave)
            )
        });
        
        Assert.That(testAddReservation, Is.EqualTo(expected));

        Assert.That(reservationBook.Entries(), Has.Exactly(size).Items);
    }*/
}
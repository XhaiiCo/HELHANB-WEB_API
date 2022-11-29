using System.Diagnostics;
using Domain;
using NuGet.Frameworks;

namespace Tests;

public class AdTests
{
    [Test]
    [TestCase("07:00:00", "06:59:59", false)]
    [TestCase("07:00:00", "07:00:01", true)]
    public void IsHour2isAfterHour1Test(string hour1, string hour2, bool expected)
    {
        Assert.That(Ad.IsHour2isAfterHour1(TimeSpan.Parse(hour1), TimeSpan.Parse(hour2)), Is.EqualTo(expected));
    }

    [Test]
    [TestCase("06:59:59", "08:00:00", "07:00:00","L'heure de départ doit être avant l'heure d'arrivée")]
    [TestCase("07:00:00", "06:59:59", "05:00:00","Heures d'arrivée incorrectes")]
    public void ValidHoursThrowsExceptionTest(string arrivalStart, string arrivalEnd, string leave, string expectedErrorMessage)
    {
        var exception = Assert.Throws<Exception>(() => Ad.ValidHours(TimeSpan.Parse(arrivalStart), TimeSpan.Parse(arrivalEnd), TimeSpan.Parse(leave)));
        
        Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    [TestCase("07:00:00", "07:00:01", "06:59:59",true)]
    public void ValidHoursThrowsExceptionTest(string arrivalStart, string arrivalEnd, string leave, bool expected)
    {
        Assert.That(Ad.ValidHours(TimeSpan.Parse(arrivalStart), TimeSpan.Parse(arrivalEnd), TimeSpan.Parse(leave)), Is.EqualTo(expected));
    }

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
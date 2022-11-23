using Domain;

namespace Tests;

public class ReservationTests
{
    [Test]
    [TestCase("25/12/2021 07:00", "01/01/2022 10:00", false)]
    [TestCase("25/12/2021 07:00", "01/01/2022 09:59:59", true)]
    [TestCase("13/01/2022 22:00", "17/01/2022 22:00", false)]
    [TestCase("13/01/2022 22:00:01", "17/01/2022 22:00", true)]
    public void IsDateAvailableTest(string arrivalDate, string leaveDate, bool expected)
    {
        List<Reservation> reservations = new List<Reservation>();

        reservations.Add(new Reservation
        {
            DateTimeRange = new DateTimeRange
            (           //01/01/2022 10:00:00
                new DateTime(2022, 01, 01, 10, 0, 0),
                        //13/01/2022 22:00:00
                new DateTime(2022, 01, 13, 22, 0, 0)
            )
        });

        Assert.That(Reservation.IsDateAvailable(reservations, new Reservation
        {
            DateTimeRange = new DateTimeRange
            (
                DateTime.Parse(arrivalDate),
                DateTime.Parse(leaveDate)
            )
        }), Is.EqualTo(expected));
    }
    
    [Test]
    [TestCase("22/11/2022 00:00", "22/11/2022 23:59:59", true)]
    [TestCase("22/11/2022 00:00", "23/11/2022 00:00", false)]
    [TestCase("22/11/2022 23:59:59", "23/11/2022 00:00:01", false)]
    public void IsLessThanOneNightTest(string arrivalDate, string leaveDate, bool expected)
    {
        Assert.That(Reservation.IsLessThanOneNight(new Reservation
        {
            DateTimeRange = new DateTimeRange(DateTime.Parse(arrivalDate),DateTime.Parse(leaveDate))
        }), Is.EqualTo(expected));
    }
    
    [Test]
    [TestCase("21/11/2022 00:00", false)]
    [TestCase("23/11/2022 00:00", true)]//Always replace with today day
    public void IsForTheSameDayTest(string arrivalDate, bool expected)
    {
        Assert.That(Reservation.IsForTheSameDay(new Reservation
        {
            DateTimeRange = new DateTimeRange
            {
                ArrivalDate = DateTime.Parse(arrivalDate)
            }
        }), Is.EqualTo(expected));
    }

    [Test]
    [TestCase("21/11/2022 00:00", true)]
    [TestCase("23/11/2022 19:00", false)]//Always replace with today day and highter hour
    [TestCase("22/11/2100 00:00", false)]
    public void IsInThePastTest(string arrivalDate, bool expected)
    {
        Assert.That(Reservation.IsInThePast(new Reservation
        {
            DateTimeRange = new DateTimeRange
            {
                ArrivalDate = DateTime.Parse(arrivalDate)
            }
        }), Is.EqualTo(expected));
    }

    [Test]
    [TestCase("22/11/2022 00:00", "22/11/2022 23:59:59", 50f, 0f)]
    [TestCase("22/11/2022 00:00", "23/11/2022 00:00", 50f, 50f)]
    [TestCase("22/11/2022 00:00", "23/11/2022 23:59:59", 50f, 50f)]
    [TestCase("22/11/2022 23:59:59", "23/11/2022 00:00:01", 50f, 50f)]
    [TestCase("22/11/2022 00:00:00", "25/11/2022 00:00:00", 87.09f, 261.27f)]
    public void ComputeReservationPriceTest(string arrivalDate, string leaveDate, float priceByNight, float expected)
    {
        Reservation r = new Reservation
        {
            DateTimeRange = new DateTimeRange
            {
                ArrivalDate = DateTime.Parse(arrivalDate),
                LeaveDate = DateTime.Parse(leaveDate)
            }
        };
        
        Assert.That(r.ComputeReservationPrice(priceByNight), Is.EqualTo(expected));
    }

    [Test]
    [TestCase("23/11/2021 19:00", "24/11/2021 23:59:59", "La date d'arrivée ne peut pas être passée")]
    [TestCase("23/11/2022 19:00", "24/11/2100 23:59:59","Une réservation doit être au minimum pour le lendemain")] //date of today
    [TestCase("24/11/2100 00:00", "24/11/2100 23:59:59", "La réservation doit au moins faire une nuit")]//a date above the actual date
    public void ValidNewReservationThrowsExceptionTest(string arrivalDate, string leaveDate, string expectedErrorMessage)
    {
        var exception = Assert.Throws<Exception>(() => Reservation.ValidNewReservation(new Reservation
        {
            DateTimeRange = new DateTimeRange
            {
                ArrivalDate = DateTime.Parse(arrivalDate),
                LeaveDate = DateTime.Parse(leaveDate)
            }
        }));
        Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    [TestCase("24/11/2022 00:00", "23/11/2100 00:00", true)]
    public void ValidNewReservationTest(string arrivalDate, string leaveDate, bool expected)
    {
        Assert.That(Reservation.ValidNewReservation(new Reservation
        {
            DateTimeRange = new DateTimeRange
            {
                ArrivalDate = DateTime.Parse(arrivalDate),
                LeaveDate = DateTime.Parse(leaveDate)
            }
        }), Is.EqualTo(expected));
    }
}
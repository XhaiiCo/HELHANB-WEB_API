using Domain;

namespace Tests;

public class ReservationTests
{
    [Test]
    [TestCase("25/12/2021 07:00:00", "01/01/2022 10:00:00", false)]
    [TestCase("25/12/2021 07:00:00", "01/01/2022 09:59:59", true)]
    [TestCase("13/01/2022 22:00:00", "17/01/2022 22:00:00", false)]
    [TestCase("13/01/2022 22:00:01", "17/01/2022 22:00:00", true)]
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
    [TestCase("22/11/2022 00:00:00", "22/11/2022 23:59:59", true)]
    [TestCase("22/11/2022 00:00:00", "23/11/2022 00:00:00", false)]
    [TestCase("22/11/2022 23:59:59", "23/11/2022 00:00:00", false)]
    public void IsLessThanOneNightTest(string arrivalDate, string leaveDate, bool expected)
    {
        Assert.That(Reservation.IsLessThanOneNight(new Reservation
        {
            DateTimeRange = new DateTimeRange(DateTime.Parse(arrivalDate),DateTime.Parse(leaveDate))
        }), Is.EqualTo(expected));
    }
    
    [Test]
    [TestCase(-1,false)]
    [TestCase(0,true)]
    [TestCase(1,false)]
    public void IsForTheSameDayTest(int dayDiff, bool expected)
    {
        var arrivalDate = DateTime.Now.AddDays(dayDiff);
        
        Assert.That(Reservation.IsForTheSameDay(new Reservation
        {
            DateTimeRange = new DateTimeRange
            {
                ArrivalDate = arrivalDate
            }
        }), Is.EqualTo(expected));
    }

    [Test]
    [TestCase(-1, true)]
    [TestCase(0, true)]
    [TestCase(1, false)]
    public void IsInThePastTest(int secondDiff, bool expected)
    {
        var arrivalDate = DateTime.Now.AddSeconds(secondDiff);
        
        Assert.That(Reservation.IsInThePast(new Reservation
        {
            DateTimeRange = new DateTimeRange
            {
                ArrivalDate = arrivalDate
            }
        }), Is.EqualTo(expected));
    }

    [Test]
    [TestCase("22/11/2022 00:00", "22/11/2022 23:59:59", 50f, 0f)]
    [TestCase("22/11/2022 00:00", "23/11/2022 00:00", 50f, 50f)]
    [TestCase("22/11/2022 00:00", "23/11/2022 23:59:59", 50f, 50f)]
    [TestCase("22/11/2022 23:59:59", "23/11/2022 00:00:00", 50f, 50f)]
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
    [TestCase(0,-1, 1,0, "La date d'arrivée ne peut pas être passée")]
    [TestCase(0,10, 1,10,"Une réservation doit être au minimum pour le lendemain")]
    [TestCase(1,0, 1,1, "La réservation doit au moins faire une nuit")]
    public void ValidNewReservationThrowsExceptionTest(int dayDiff1, int secondDiff1, int dayDiff2, int secondDiff2, string expectedErrorMessage)
    {
        var arrivalDate = DateTime.Now.AddDays(dayDiff1).AddSeconds(secondDiff1);
        var leaveDate = DateTime.Now.AddDays(dayDiff2).AddSeconds(secondDiff2);
        
        var exception = Assert.Throws<Exception>(() => Reservation.ValidNewReservation(new Reservation
        {
            DateTimeRange = new DateTimeRange
            {
                ArrivalDate = arrivalDate,
                LeaveDate = leaveDate
            }
        }));
        Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    [TestCase(1," 00:00",2, " 00:00", true)]
    public void ValidNewReservationTest(int dayDiff1, string arrivalHour, int dayDiff2, string leaveHour, bool expected)
    {
        var arrivalDate = DateTime.Now.AddDays(dayDiff1).ToString("dd/MM/yyyy");;
        var leaveDate = DateTime.Now.AddDays(dayDiff2).ToString("dd/MM/yyyy");;
        
        Assert.That(Reservation.ValidNewReservation(new Reservation
        {
            DateTimeRange = new DateTimeRange
            {
                ArrivalDate = DateTime.Parse(arrivalDate + arrivalHour),
                LeaveDate = DateTime.Parse(leaveDate + leaveHour)
            }
        }), Is.EqualTo(expected));
    }
}
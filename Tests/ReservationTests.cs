using Domain;

namespace Tests;

public class ReservationTests
{
    [Test]
    [TestCase("01/01/2022 07:00", "01/01/2022 11:00", false)]
    [TestCase("25/12/2021 07:00", "01/01/2022 07:00", true)]
    [TestCase("13/01/2022 21:00", "17/01/2022 22:00", false)]
    [TestCase("13/01/2022 23:00", "17/01/2022 22:00", true)]
    //[TestCase("25/12/2022 07:00", "01/01/2022 07:00", false)]
    public void IsDateAvailableTest(string dateArrival, string dateLeave, bool expected)
    {
        List<Reservation> reservations = new List<Reservation>();
        
        reservations.Add(new Reservation
        {
            ArrivalDate = new DateTime(2022, 01, 01, 10, 0, 0), 
            LeaveDate = new DateTime(2022, 01, 13, 22, 0, 0)
        });
        
        Assert.That(Reservation.IsDateAvailable(reservations, new Reservation
        {
            ArrivalDate = DateTime.Parse(dateArrival), LeaveDate = DateTime.Parse(dateLeave)
        }), Is.EqualTo(expected));
        
        
    }
}
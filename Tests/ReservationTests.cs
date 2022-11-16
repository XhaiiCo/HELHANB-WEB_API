using Domain;

namespace Tests;

public class ReservationTests
{
    [Test]
    [TestCase("01/01/2022", "13/01/2022", false)]
    [TestCase("14/01/2022", "17/01/2022", true)]
    public void IsDateAvailableTest(string dateArrival, string dateLeave, bool expected)
    {
        List<Reservation> reservations = new List<Reservation>();
        
        reservations.Add(new Reservation
        {
            ArrivalDate = new DateOnly(2022, 01, 01), 
            LeaveDate = new DateOnly(2022, 01, 13)
        });
        
        Assert.That(Reservation.IsDateAvailable(reservations, new Reservation
        {
            ArrivalDate = DateOnly.Parse(dateArrival), LeaveDate = DateOnly.Parse(dateLeave)
        }), Is.EqualTo(expected));
    }
}
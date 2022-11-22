using Domain;

namespace Tests;

public class DateTimeRangeTests
{
    [Test]
    [TestCase("22/11/2022 00:00", "22/11/2022 23:59:59", 0)]
    [TestCase("22/11/2022 00:00", "23/11/2022 00:00", 1)]
    [TestCase("22/11/2022 00:00", "23/11/2022 23:59:59", 1)]
    [TestCase("22/11/2022 23:59:59", "23/11/2022 00:00:01", 1)]
    public void ComputeNbNightTest(string dateArrival, string dateLeave, int expected)
    {
        DateTimeRange dtr = new DateTimeRange(DateTime.Parse(dateArrival), DateTime.Parse(dateLeave));
        
        Assert.That(dtr.ComputeNbNight(), Is.EqualTo(expected));
    }

    /*
    public void ArrivalDateSetterTest(DateTime arrivalDate)
    {
        
    }*/
    
    
}
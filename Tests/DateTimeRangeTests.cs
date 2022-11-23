using Domain;

namespace Tests;

public class DateTimeRangeTests
{
    [Test]
    [TestCase("22/11/2022 00:00", "22/11/2022 23:59:59", 0)]
    [TestCase("22/11/2022 00:00", "23/11/2022 00:00", 1)]
    [TestCase("22/11/2022 00:00", "23/11/2022 23:59:59", 1)]
    [TestCase("22/11/2022 23:59:59", "23/11/2022 00:00:01", 1)]
    public void ComputeNbNightTest(string arrivalDate, string leaveDate, int expected)
    {
        DateTimeRange dtr = new DateTimeRange(DateTime.Parse(arrivalDate), DateTime.Parse(leaveDate));
        
        Assert.That(dtr.ComputeNbNight(), Is.EqualTo(expected));
    }
    
    [Test]
    [TestCase("01/01/2023 10:00:00", "La date d'arrivée ne peut pas être après la date de départ")]
    [TestCase("01/01/2022 10:00:00", "La date d'arrivée ne peut pas être après la date de départ")]
    public void ArrivalDateSetterThrowsExceptionTest(string arrivalDate, string expectedErrorMessage)
    {
        DateTimeRange dtr = new DateTimeRange
        {
            //01/01/2022 10:00:00
            LeaveDate = new DateTime(2022, 01, 01, 10, 0, 0)
        };
        
        var exception = Assert.Throws<ArgumentException>(() => dtr.ArrivalDate = DateTime.Parse(arrivalDate));
        Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
    }
    
    [Test]
    [TestCase("01/01/2021 10:00:00", "La date de départ ne peut pas être avant la date d'arrivée")]
    [TestCase("01/01/2022 10:00:00", "La date de départ ne peut pas être avant la date d'arrivée")]
    public void LeaveDateSetterThrowsExceptionTest(string leaveDate, string expectedErrorMessage)
    {
        DateTimeRange dtr = new DateTimeRange
        {
            //01/01/2022 10:00:00
            ArrivalDate = new DateTime(2022, 01, 01, 10, 0, 0)
        };
        
        var exception = Assert.Throws<ArgumentException>(() => dtr.LeaveDate = DateTime.Parse(leaveDate));
        Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
    }
}
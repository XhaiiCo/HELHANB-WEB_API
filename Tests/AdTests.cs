using Domain;

namespace Tests;

public class AdTests
{
    [Test]
    [TestCase("07:00:00", "06:59:59", false)]
    [TestCase("07:00:00", "07:00:01", true)]
    public void IsHour2IsAfterHour1Test(string hour1, string hour2, bool expected)
    {
        Assert.That(Ad.IsHour2IsAfterHour1(TimeSpan.Parse(hour1), TimeSpan.Parse(hour2)), Is.EqualTo(expected));
    }

    [Test]
    [TestCase("06:59:59", "08:00:00", "07:00:00", "L'heure de départ doit être avant l'heure d'arrivée")]
    [TestCase("07:00:00", "06:59:59", "05:00:00", "Heures d'arrivée incorrectes")]
    public void ValidHoursThrowsExceptionTest(string arrivalStart, string arrivalEnd, string leave,
        string expectedErrorMessage)
    {
        var exception = Assert.Throws<Exception>(() =>
            Ad.ValidHours(TimeSpan.Parse(arrivalStart), TimeSpan.Parse(arrivalEnd), TimeSpan.Parse(leave)));

        Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    [TestCase("07:00:00", "07:00:01", "06:59:59", true)]
    public void ValidHoursThrowsExceptionTest(string arrivalStart, string arrivalEnd, string leave, bool expected)
    {
        Assert.That(Ad.ValidHours(TimeSpan.Parse(arrivalStart), TimeSpan.Parse(arrivalEnd), TimeSpan.Parse(leave)),
            Is.EqualTo(expected));
    }
}
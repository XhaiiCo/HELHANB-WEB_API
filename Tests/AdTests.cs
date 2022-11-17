﻿using Domain;

namespace Tests;


public class AdTests
{
    [Test]
    [TestCase("01/01/2022 07:00", "01/01/2022 11:00", false, 1)]
    [TestCase("25/12/2021 07:00", "01/01/2022 07:00", true, 2)]
    [TestCase("13/01/2022 21:00", "17/01/2022 22:00", false, 1)]
    [TestCase("13/01/2022 23:00", "17/01/2022 22:00", true, 2)]
    public void AddReservation(string dateArrival, string dateLeave, bool expected, int size)
    {
        Ad ad = new Ad();

        ad.AddReservation(new Reservation{
            ArrivalDate = new DateTime(2022, 01, 01, 10, 0, 0), 
            LeaveDate = new DateTime(2022, 01, 13, 22, 0, 0)
        });

        Assert.That(ad.AddReservation(new Reservation
        {
            ArrivalDate = DateTime.Parse(dateArrival), LeaveDate = DateTime.Parse(dateLeave)
        }), Is.EqualTo(expected));
        
        Assert.That(ad.Reservations, Has.Exactly(size).Items);
    }
    
    
}
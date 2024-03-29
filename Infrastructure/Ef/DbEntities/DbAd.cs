﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Ef.DbEntities;

public class DbAd
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime Created { get; set; }
    
    public float PricePerNight { get; set; }
    
    public string Description { get; set; }
    
    public int NumberOfPersons { get; set; }
    
    public int NumberOfBedrooms { get; set; }
    
    public string Street { get; set; }
    
    public int PostalCode { get; set; }
    
    public string Country { get; set; }
    
    public string City { get; set; }
    
    public int UserId { get; set; }
    
    public int AdStatusId { get; set; }
    
    public TimeSpan ArrivalTimeRangeStart { get; set; }
    
    public TimeSpan ArrivalTimeRangeEnd { get; set; }
    
    public TimeSpan LeaveTime { get; set; }
    
    public string AdSlug { get; set; }
}
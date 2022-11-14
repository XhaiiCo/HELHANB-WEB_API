using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository.Ad;

public class AdRepository : IAdRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public AdRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }


    /*public IEnumerable<DbAd> FetchAll()
    {
        using var context = _contextProvider.NewContext();
        return context.Ads.ToList();
    }*/

    public DbAd Create(int id,string name, float price, string description,int numberOfPersons,int numberOfBedrooms, string street, int postalCode, string country, string city)
    {
        using var context = _contextProvider.NewContext();
        var ad = new DbAd
        {
            Id= id, Name = name, PricePerNight = price, Description = description,NumberOfPersons = numberOfPersons, NumberOfBedrooms = numberOfBedrooms,Street = street,PostalCode = postalCode,
            Country = country, City = city
        };
        context.Ads.Add(ad);
        context.SaveChanges();
        return ad;

    }

    /*public DbAd FetchByCountry(string country)
    {
        using var context = _contextProvider.NewContext();
        var ad = context.Ads.FirstOrDefault(a => a.Country == country);

        //if (ad == null)
        
        
    }*/
}
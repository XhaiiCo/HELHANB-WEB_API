using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Ef.Repository.Ad;

public class AdRepository : IAdRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public AdRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public IEnumerable<DbAd> FetchAll(FilteringAd filter)
    {
        using var context = _contextProvider.NewContext();

        return filter.StatusId.HasValue
            ? context.Ads.Where(ad => ad.AdStatusId == filter.StatusId).ToList()
            : context.Ads.ToList();
    }

    public IEnumerable<DbAd> FetchRange(int offset, int limit, FilteringAd filter)
    {
        using var context = _contextProvider.NewContext();

        return filter.StatusId.HasValue
            ? context.Ads.Where(ad => ad.AdStatusId == filter.StatusId).Skip(offset).Take(limit).ToList()
            : context.Ads.Skip(offset).Take(limit).ToList();
    }

    public DbAd Create(DbAd ad)
    {
        using var context = _contextProvider.NewContext();

        context.Ads.Add(ad);
        context.SaveChanges();
        return ad;
    }

    public DbAd FetchById(int id)
    {
        using var context = _contextProvider.NewContext();
        var ad = context.Ads.FirstOrDefault(ad => ad.Id == id);

        if (ad == null)
            throw new KeyNotFoundException($"Ad with id {id} has not been found");

        return ad;
    }

    public DbAd Delete(DbAd ad)
    {
        using var context = _contextProvider.NewContext();

        context.Ads.Remove(ad);
        context.SaveChanges();

        return ad;
    }

    public DbAd Update(DbAd ad)
    {
        using var context = _contextProvider.NewContext();

        context.Attach(ad);
        context.Entry(ad).State = EntityState.Modified;
        context.SaveChanges();

        return ad;
    }

    /*public DbAd FetchByCountry(string country)
    {
        using var context = _contextProvider.NewContext();
        var ad = context.Ads.FirstOrDefault(a => a.Country == country);

        //if (ad == null)
        
        
    }*/

    public int Count(FilteringAd filter)
    {
        using var context = _contextProvider.NewContext();

        return filter.StatusId.HasValue
            ? context.Ads.Count(ad => ad.AdStatusId == filter.StatusId)
            : context.Ads.Count();
    }
}
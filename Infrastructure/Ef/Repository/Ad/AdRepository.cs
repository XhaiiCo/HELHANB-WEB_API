//using Domain;

using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Reservation;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Ef.Repository.Ad;

public class AdRepository : IAdRepository
{
    private readonly HelhanbContextProvider _contextProvider;
    private readonly IReservationRepository _reservationRepository;

    public AdRepository(HelhanbContextProvider contextProvider, IReservationRepository reservationRepository)
    {
        _contextProvider = contextProvider;
        _reservationRepository = reservationRepository;
    }

    public IEnumerable<DbAd> FetchAll(FilteringAd filter)
    {
        using var context = _contextProvider.NewContext();

        var ads = context.Ads.Where(ad => filter.StatusId == null || ad.AdStatusId == filter.StatusId).ToList();
        
        if (filter.Offset != null && filter.Limit != null)
        {
            return ads.Skip(Convert.ToInt32(filter.Offset)).Take(Convert.ToInt32(filter.Limit));
        }

        return ads;
    }

    public IEnumerable<string> FetchDistinctsCountries()
    {
        using var context = _contextProvider.NewContext();

        return context.Ads.Select(item => item.Country).Distinct().ToList();
    }

    public IEnumerable<string> FetchDistinctsCitiesByCountry(string country)
    {
        using var context = _contextProvider.NewContext();

        return context.Ads.Where(item => item.Country == country).Select(item => item.City).Distinct().ToList();
    }

    public IEnumerable<DbAd> FetchRange(FilteringAd filter)
    {
        if (filter.Limit == null || filter.Offset == null)
            throw new Exception("Limit and offset cannot be null");

        using var context = _contextProvider.NewContext();

        return context.Ads.Where(ad =>
                (filter.StatusId == null || ad.AdStatusId == filter.StatusId)
                && (filter.Country == null || ad.Country == filter.Country)
                && (filter.City == null || ad.City == filter.City)
                && (filter.PricePerNight == null || ad.PricePerNight <= filter.PricePerNight)
                && (filter.NumberOfPersons == null ||
                    ad.NumberOfPersons >= filter.NumberOfPersons))
            .Skip((int)filter.Offset).Take((int)filter.Limit).ToList();
    }

    public IEnumerable<DbAd> FetchByUserId(int id)
    {
        using var context = _contextProvider.NewContext();

        return context.Ads.Where(ad => ad.UserId == id).ToList();
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

    public DbAd FetchBySlug(string slug)
    {
        using var context = _contextProvider.NewContext();
        var ad = context.Ads.FirstOrDefault(ad => ad.AdSlug == slug);

        if (ad == null)
            throw new KeyNotFoundException($"Ad with slug {slug} has not been found");

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

    public IEnumerable<DbAd> FilterAds(FilteringAd filter)
    {
        using var context = _contextProvider.NewContext();
        
        return context.Ads.Where(ad => (filter.StatusId == null || ad.AdStatusId == filter.StatusId)
                                       && (filter.Name == null || ad.Name.Contains(filter.Name.Trim()))
                                       && (filter.Country == null || ad.Country == filter.Country)
                                       && (filter.City == null || ad.City == filter.City)
                                       && (filter.PricePerNight == null || ad.PricePerNight <= filter.PricePerNight)
                                       && (filter.NumberOfPersons == null ||
                                           ad.NumberOfPersons >= filter.NumberOfPersons)).ToList();
    }
}
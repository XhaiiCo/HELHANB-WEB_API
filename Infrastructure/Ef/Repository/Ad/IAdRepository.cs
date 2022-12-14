using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.Ad;

public interface IAdRepository
{
    IEnumerable<DbAd> FetchAll(FilteringAd filter);
    IEnumerable<string> FetchDistinctsCountries();
    IEnumerable<string> FetchDistinctsCitiesByCountry(string country);
    
    IEnumerable<DbAd> FetchRange(FilteringAd filter);

    DbAd Create(DbAd ad);

    DbAd FetchById(int id);

    DbAd FetchBySlug(string slug);
    IEnumerable<DbAd> FetchByUserId(int id);

    DbAd Delete(DbAd ad);
    DbAd Update(DbAd ad);
    
    public IEnumerable<DbAd> FilterAds(FilteringAd filter);
}
using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.Ad;

public interface IAdRepository
{
    //  IEnumerable <DbAd> FetchAll();

    DbAd Create(DbAd ad);

    DbAd FetchById(int id);

    DbAd Delete(DbAd ad);

    // DbAd FetchByCountry(string country);
}
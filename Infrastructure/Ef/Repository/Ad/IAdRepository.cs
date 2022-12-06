﻿using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.Ad;

public interface IAdRepository
{
    IEnumerable<DbAd> FetchAll(FilteringAd filter);
    IEnumerable<string> FetchDistinctByCountry();
    IEnumerable<string> FetchByCountryDistinctCity(string country);
    
    IEnumerable<DbAd> FetchRange(int offset, int limit, FilteringAd filter);

    DbAd Create(DbAd ad);

    DbAd FetchById(int id);
    IEnumerable<DbAd> FetchByUserId(int id);

    DbAd Delete(DbAd ad);
    DbAd Update(DbAd ad);

    // DbAd FetchByCountry(string country);
    int Count(FilteringAd filter);
}
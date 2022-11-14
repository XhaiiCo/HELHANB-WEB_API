using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.Ad;

public interface IAdRepository
{
    //  IEnumerable <DbAd> FetchAll();

    DbAd Create(int id,string name, float price, string description, int numberOfPerson,int numberOfBedrooms ,string street, int postalCode, string country,
        string city);

    // DbAd FetchByCountry(string country);
}
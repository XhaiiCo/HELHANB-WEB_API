using Infrastructure.Ef.DbEntities;

namespace Application.Services.Ad;

public interface IAdService
{
    Domain.Ad FetchById(int id);
    
    IEnumerable<Domain.Ad> FetchAll();

    Domain.Ad MapToAd(DbAd dbAd);
}
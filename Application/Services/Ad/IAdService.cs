using Infrastructure.Ef.DbEntities;

namespace Application.Services.Ad;

public interface IAdService
{
    Domain.Ad FetchById(int id);
    
    IEnumerable<Domain.Ad> FetchAll();
    IEnumerable<Domain.Ad> FetchRange(int offset, int limit);

    Domain.Ad MapToAd(DbAd dbAd);
}
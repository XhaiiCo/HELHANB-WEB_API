using Application.UseCases.Ads.Dtos;
using Infrastructure.Ef.DbEntities;

namespace Application.Services.Ad;

public interface IAdService
{
    Domain.Ad FetchById(int id);
    IEnumerable<Domain.Ad> FetchByUserId(int id);
    
    IEnumerable<Domain.Ad> FetchAll(DtoInputFilteringAds dto);
    IEnumerable<Domain.Ad> FetchRange(int offset, int limit, DtoInputFilteringAds filter);

    Domain.Ad MapToAd(DbAd dbAd);
}
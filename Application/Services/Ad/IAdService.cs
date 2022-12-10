using Application.UseCases.Ads.Dtos;
using Infrastructure.Ef.DbEntities;

namespace Application.Services.Ad;

public interface IAdService
{
    Domain.Ad FetchById(int id);
    Domain.Ad FetchBySlug(string slug);
    IEnumerable<Domain.Ad> FetchByUserId(int id);
    
    IEnumerable<Domain.Ad> FetchAll(DtoInputFilteringAds dto);
    IEnumerable<Domain.Ad> FetchRange(DtoInputFilteringAds filter);

    Domain.Ad MapToAd(DbAd dbAd);
}
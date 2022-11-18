using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseFetchAdById : IUseCaseParameterizedQuery<DtoOutputAd, int>
{
    private readonly IAdRepository _adRepository;

    public UseCaseFetchAdById(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }
    public DtoOutputAd Execute(int id)
    {
        var ad = _adRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputAd>(ad);
    }
}
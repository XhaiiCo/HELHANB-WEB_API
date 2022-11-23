using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.AdPicture;

namespace Application.UseCases.Ads;

public class UseCaseAddPictureAd : IUseCaseWriter<DtoOutputAdPicture, DtoInputAddPictureAd>
{
    private readonly IAdPictureRepository _adPictureRepository;

    public UseCaseAddPictureAd(IAdPictureRepository adPictureRepository)
    {
        _adPictureRepository = adPictureRepository;
    }

    public DtoOutputAdPicture Execute(DtoInputAddPictureAd input)
    {
        var dbAdPicture = Mapper.GetInstance().Map<DbAdPicture>(input);
        var result = _adPictureRepository.Create(dbAdPicture);

        return Mapper.GetInstance().Map<DtoOutputAdPicture>(result);
    }
}
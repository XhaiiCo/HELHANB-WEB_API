using API.Utils.Picture;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.AdPicture;

namespace Application.UseCases.Ads;

public class UseCaseDeleteAd : IUseCaseParameterizedQuery<DtoOutputAd, string>
{
    private readonly IAdRepository _adRepository;
    private readonly IAdPictureRepository _adPictureRepository;
    private readonly IPictureService _pictureService;

    public UseCaseDeleteAd(IAdRepository adRepository, IAdPictureRepository adPictureRepository, IPictureService pictureService)
    {
        _adRepository = adRepository;
        _adPictureRepository = adPictureRepository;
        _pictureService = pictureService;
    }

    public DtoOutputAd Execute(string slug)
    {
        var ad = _adRepository.FetchBySlug(slug);

        var dbAd = Mapper.GetInstance().Map<DbAd>(ad);

        var dbAdPictures = _adPictureRepository.FetchByAdId(dbAd.Id);
        
        foreach (var dbAdPicture in dbAdPictures)
        {
            _adPictureRepository.Delete(dbAdPicture);
                
            _pictureService.RemoveFile(dbAdPicture.Path);
        }
        
        _adRepository.Delete(dbAd);
        
        return Mapper.GetInstance().Map<DtoOutputAd>(ad);
    }
}
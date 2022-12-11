using Application.Services.Ad;
using Application.Services.User;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseUpdateStatusAd : IUseCaseWriter<DtoOutputAd, DtoInputUpdateStatusAd>
{
    private readonly IAdRepository _adRepository;
    private readonly IAdService _adService;
    private readonly IUserService _userService;

    public UseCaseUpdateStatusAd(
        IAdRepository adRepository,
        IAdService adService,
        IUserService userService
    )
    {
        _adRepository = adRepository;
        _adService = adService;
        _userService = userService;
    }

    public DtoOutputAd Execute(DtoInputUpdateStatusAd input)
    {
        //Check the user role
        var user = _userService.FetchById(input.UserId);
        var dbAd = _adRepository.FetchBySlug(input.AdSlug);

        if (user.Role.Name == "hote")
        {
            if (dbAd.UserId != user.Id)
                throw new Exception("Vous n'avez pas le droit de modifier cette annonce");
            if (dbAd.AdStatusId is not (3 or 4))
                throw new Exception("Vous n'avez pas le droit de changer le rôle");
            if (input.StatusId is not (4 or 3))
                throw new Exception("Vous n'avez pas le droit de mettre ce rôle");
        }

        dbAd.AdStatusId = input.StatusId;

        var result = _adRepository.Update(dbAd);

        return Mapper.GetInstance().Map<DtoOutputAd>(_adService.MapToAd(result));
    }
}
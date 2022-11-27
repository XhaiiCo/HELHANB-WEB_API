using API.Utils.Picture;
using Application.Services.Ad;
using Application.UseCases.Ads;
using Application.UseCases.Ads.Dtos;
using Application.UseCases.Reservations;
using Application.UseCases.Reservations.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Ads;

[ApiController]
[Route("api/v1/ad")]
public class AdController : ControllerBase
{
    private readonly IAdService _adService;
    private readonly IPictureService _pictureService;

    private readonly UseCaseCreateAd _useCaseCreateAd;
    private readonly UseCaseDeleteAd _useCaseDeleteAd;
    private readonly UseCaseCreateReservation _useCaseCreateReservation;
    private readonly UseCaseFetchAllAds _useCaseFetchAllAds;
    private readonly UseCaseAddPictureAd _useCaseAddPictureAd;
    private readonly UseCaseFetchAdById _useCaseFetchAdById;
    private readonly UseCaseCountValidatedAds _useCaseCountValidatedAds;
    private readonly UseCaseFetchAdsForPagination _useCaseFetchAdsForPagination;
    private readonly UseCaseUpdateStatusAd _useCaseUpdateStatusAd ;

    public AdController(UseCaseCreateAd useCaseCreateAd, UseCaseDeleteAd useCaseDeleteAd,
        UseCaseCreateReservation useCaseCreateReservation, UseCaseFetchAllAds useCaseFetchAllAds, IAdService adService,
        IPictureService pictureService, UseCaseAddPictureAd useCaseAddPictureAd, UseCaseFetchAdById useCaseFetchAdById,
        UseCaseCountValidatedAds useCaseCountValidatedAds, UseCaseFetchAdsForPagination useCaseFetchAdsForPagination, UseCaseUpdateStatusAd useCaseUpdateStatusAd)
    {
        _useCaseCreateAd = useCaseCreateAd;
        _useCaseDeleteAd = useCaseDeleteAd;
        _useCaseCreateReservation = useCaseCreateReservation;
        _useCaseFetchAllAds = useCaseFetchAllAds;
        _adService = adService;
        _pictureService = pictureService;
        _useCaseAddPictureAd = useCaseAddPictureAd;
        _useCaseFetchAdById = useCaseFetchAdById;
        _useCaseCountValidatedAds = useCaseCountValidatedAds;
        _useCaseFetchAdsForPagination = useCaseFetchAdsForPagination;
        _useCaseUpdateStatusAd = useCaseUpdateStatusAd;
    }


    [HttpPost]
    [Authorize(Roles = "hote")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputAd> CreateAd(DtoInputCreateAd dto)
    {
        //Check that this is the id of the logged in user
        if ("" + dto.UserId != User.Identity?.Name) return Unauthorized();

        try
        {
            return Ok(_useCaseCreateAd.Execute(dto));
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "hote")]
    [Route("{id:int}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputAdPicture> AddPictureForAd(int id, IFormFile? picture)
    {
        //Check that this is the id of the logged in user
        var ad = _adService.FetchById(id);
        if ("" + ad.Owner.Id != User.Identity?.Name) return Unauthorized();

        try
        {
            //If the protilePicture is null remove it
            if (picture == null)
            {
                return Unauthorized("Il faut une photo");
            }

            var basePath = "\\Upload\\AdPictures\\" + ad.Id + "\\";

            //Check the file type
            if (!_pictureService.ValidPictureType(picture.ContentType))
            {
                return Unauthorized("Extension d'image invalide acceptés: jpeg, png, webp");
            }

            //Create a unique file name
            var fileName = _pictureService.GenerateUniqueFileName(id, picture.FileName);

            //Update the ad 
            var dtoInputAddPictureAd = new DtoInputAddPictureAd
            {
                Path = basePath + fileName,
                AdId = ad.Id
            };
            var dtoOutputAdPicture = _useCaseAddPictureAd.Execute(dtoInputAddPictureAd);

            //Upload the new picture
            _pictureService.UploadPicture(basePath, fileName, picture);
            return Ok(dtoOutputAdPicture);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "administrateur")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputAd> DeleteAd(int id)
    {
        try
        {
            var ad = _useCaseDeleteAd.Execute(id);
            return Ok(ad);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DtoOutputAd>> FetchAll([FromQuery] int? statusId)
    {
        return Ok(_useCaseFetchAllAds.Execute(new DtoInputFilteringAds
        {
            StatusId = statusId 
        }));
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputAdWithReservations> FetchById(int id)
    {
        return Ok(_useCaseFetchAdById.Execute(id));
    }

    [HttpPost]
    [Route("{id:int}/reservation")]
    [Authorize(Roles = "utilisateur,hote")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputReservation> CreateReservation(int id, DtoInputCreateReservation dto)
    {
        //Check that this is the id of the logged in user
        if ("" + dto.RenterId != User.Identity?.Name) return Unauthorized();

        dto.AdId = id;

        try
        {
            return StatusCode(201, _useCaseCreateReservation.Execute(dto));
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpGet]
    [Route("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<int> CountValidatedAds()
    {
        return Ok(_useCaseCountValidatedAds.Execute());
    }

    [HttpGet]
    [Route("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DtoOutputAdsSummary>> FetchForPagination([FromQuery] int? limit,
        [FromQuery] int? offset)
    {
        return Ok(_useCaseFetchAdsForPagination.Execute(new DtoInputFilterAdsForPagination
        {
            Limit = limit,
            Offset = offset
        }));
    }

    [HttpPut]
    [Route("status")]
    [Authorize(Roles = "administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputAd> UpdateStatus(DtoInputUpdateStatusAd dto)
    {
        return Ok(_useCaseUpdateStatusAd.Execute(dto));
    }

}
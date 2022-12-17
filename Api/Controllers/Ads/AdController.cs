using Application.Services.Ad;
using Application.UseCases;
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

    private readonly UseCaseCreateAd _useCaseCreateAd;
    private readonly UseCaseDeleteAd _useCaseDeleteAd;
    private readonly UseCaseCreateReservation _useCaseCreateReservation;
    private readonly UseCaseFetchAllAds _useCaseFetchAllAds;
    private readonly UseCaseFetchAdBySlug _useCaseFetchAdBySlug;
    private readonly UseCaseCountValidatedAds _useCaseCountValidatedAds;
    private readonly UseCaseFetchAdsForPagination _useCaseFetchAdsForPagination;
    private readonly UseCaseUpdateStatusAd _useCaseUpdateStatusAd;
    private readonly UseCaseFetchByUserIdAd _useCaseFetchByUserIdAd;
    private readonly UseCaseUpdateAd _useCaseUpdateAd;
    private readonly UseCaseFetchMyReservations _useCaseFetchMyReservations;
    private readonly UseCaseRemoveReservation _useCaseRemoveReservation;
    private readonly UseCaseFetchDistinctsCountries _useCaseFetchDistinctsCountries;
    private readonly UseCaseFetchDistinctsCitiesByCountry _useCaseFetchDistinctsCitiesByCountry;
    private readonly UseCaseFetchAllReservationByAd _useCaseFetchAllReservationByAd;
    private readonly UseCaseConfirmReservation _useCaseConfirmReservation;
    private readonly UseCaseRefuseReservation _useCaseRefuseReservation;

    public AdController(
        UseCaseCreateAd useCaseCreateAd,
        UseCaseDeleteAd useCaseDeleteAd,
        UseCaseCreateReservation useCaseCreateReservation,
        UseCaseFetchAllAds useCaseFetchAllAds,
        UseCaseFetchAdBySlug useCaseFetchAdBySlug,
        UseCaseCountValidatedAds useCaseCountValidatedAds,
        UseCaseFetchAdsForPagination useCaseFetchAdsForPagination,
        UseCaseUpdateStatusAd useCaseUpdateStatusAd,
        UseCaseFetchByUserIdAd useCaseFetchByUserIdAd,
        UseCaseUpdateAd useCaseUpdateAd,
        UseCaseFetchMyReservations useCaseFetchMyReservations,
        UseCaseRemoveReservation useCaseRemoveReservation,
        UseCaseFetchDistinctsCountries useCaseFetchDistinctsCountries,
        UseCaseFetchDistinctsCitiesByCountry useCaseFetchDistinctsCitiesByCountry,
        UseCaseFetchAllReservationByAd useCaseFetchAllReservationByAd,
        IAdService adService,
        UseCaseConfirmReservation useCaseConfirmReservation,
        UseCaseRefuseReservation useCaseRefuseReservation
    )
    {
        _useCaseCreateAd = useCaseCreateAd;
        _useCaseDeleteAd = useCaseDeleteAd;
        _useCaseCreateReservation = useCaseCreateReservation;
        _useCaseFetchAllAds = useCaseFetchAllAds;
        _useCaseFetchAdBySlug = useCaseFetchAdBySlug;
        _useCaseCountValidatedAds = useCaseCountValidatedAds;
        _useCaseFetchAdsForPagination = useCaseFetchAdsForPagination;
        _useCaseUpdateStatusAd = useCaseUpdateStatusAd;
        _useCaseFetchByUserIdAd = useCaseFetchByUserIdAd;
        _useCaseUpdateAd = useCaseUpdateAd;
        _useCaseFetchMyReservations = useCaseFetchMyReservations;
        _useCaseRemoveReservation = useCaseRemoveReservation;
        _useCaseFetchDistinctsCountries = useCaseFetchDistinctsCountries;
        _useCaseFetchDistinctsCitiesByCountry = useCaseFetchDistinctsCitiesByCountry;
        _useCaseFetchAllReservationByAd = useCaseFetchAllReservationByAd;
        _adService = adService;
        _useCaseConfirmReservation = useCaseConfirmReservation;
        _useCaseRefuseReservation = useCaseRefuseReservation;
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

    [HttpDelete]
    [Route("{slug}")]
    [Authorize(Roles = "administrateur,super-administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputAd> DeleteAd(string slug)
    {
        try
        {
            var ad = _useCaseDeleteAd.Execute(slug);
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
    [Route("{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputAdWithReservations> FetchBySlug(string slug)
    {
        return Ok(_useCaseFetchAdBySlug.Execute(slug));
    }

    [HttpGet]
    [Authorize(Roles = "hote")]
    [Route("myAds")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputMyAdsAd> FetchByUserId()
    {
        if (User.Identity?.Name == null) return Unauthorized();

        var userId = Int32.Parse(User.Identity.Name);

        return Ok(_useCaseFetchByUserIdAd.Execute(userId));
    }

    [HttpPost]
    [Route("reservation")]
    [Authorize(Roles = "utilisateur,hote")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputReservation> CreateReservation(DtoInputCreateReservation dto)
    {
        var renterId = int.Parse(User.Identity?.Name);
        dto.RenterId = renterId;

        try
        {
            var result = _useCaseCreateReservation.Execute(dto);
            return StatusCode(201, result);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpGet]
    [Route("myReservations")]
    [Authorize(Roles = "utilisateur,hote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<DtoOutputReservation>> FetchMyReservations()
    {
        if (User.Identity?.Name == null) return Unauthorized();

        var userId = Int32.Parse(User.Identity.Name);
        return Ok(_useCaseFetchMyReservations.Execute(userId));
    }

    [HttpGet]
    [Route("{adSlug}/reservation")]
    [Authorize(Roles = "hote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DtoOutputMyAdReservation>> FetchAllReservationByAd(string adSlug)
    {
        //Check the ad belong to the hote who make the request
        var ad = _adService.FetchBySlug(adSlug);
        if (ad.Owner.Id + "" != User.Identity?.Name) return Unauthorized();

        return Ok(_useCaseFetchAllReservationByAd.Execute(adSlug));
    }

    [HttpGet]
    [Authorize(Roles = "administrateur,super-administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DtoOutputAd>> FetchAll(
        [FromQuery] int? limit,
        [FromQuery] int? offset, 
        [FromQuery] int? statusId)
    {
        return Ok(_useCaseFetchAllAds.Execute(new DtoInputFilteringAds
        {
            Limit = limit,
            Offset = offset,
            StatusId = statusId
        }));
    }

    [HttpGet]
    [Route("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<int> CountValidatedAds(
        [FromQuery] int? statusId,
        [FromQuery] string? country,
        [FromQuery] string? city,
        [FromQuery] float? pricePerNight,
        [FromQuery] int? numberOfPersons,
        [FromQuery] string? arrivalDate,
        [FromQuery] string? leaveDate
    )
    {
        return Ok(_useCaseCountValidatedAds.Execute(new DtoInputFilteringAds
        {
            StatusId = statusId,
            Country = country,
            City = city,
            PricePerNight = pricePerNight,
            NumberOfPersons = numberOfPersons,
            ArrivalDate = arrivalDate,
            LeaveDate = leaveDate
        }));
    }
    
    [HttpGet]
    [Route("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DtoOutputAdsSummary>> FetchSummaryForPagination(
        [FromQuery] int? limit,
        [FromQuery] int? offset,
        [FromQuery] int? statusId,
        [FromQuery] string? country,
        [FromQuery] string? city,
        [FromQuery] float? pricePerNight,
        [FromQuery] int? numberOfPersons,
        [FromQuery] string? arrivalDate,
        [FromQuery] string? leaveDate
    )
    {
        return Ok(_useCaseFetchAdsForPagination.Execute(new DtoInputFilteringAds
        {
            Limit = limit,
            Offset = offset,

            StatusId = statusId,
            Country = country,
            City = city,
            PricePerNight = pricePerNight,
            NumberOfPersons = numberOfPersons,
            ArrivalDate = arrivalDate,
            LeaveDate = leaveDate
        }));
    }

    [HttpPut]
    [Route("status")]
    [Authorize(Roles = "administrateur,super-administrateur,hote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputAd> UpdateStatus(DtoInputUpdateStatusAd dto)
    {
        dto.UserId = int.Parse(User.Identity?.Name);

        try
        {
            return Ok(_useCaseUpdateStatusAd.Execute(dto));
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPut]
    [Route("adUpdate")]
    [Authorize(Roles = "hote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputAd> UpdateAd(DtoInputUpdateAd dto) //<DtoOutputAd>
    {
        return Ok(_useCaseUpdateAd.Execute(dto));
    }

    [HttpPut]
    [Route("confirmReservation")]
    [Authorize(Roles = "hote")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds> ConfirmReservation(DtoInputAdReservationMyAds dto)
    {
        dto.userId = int.Parse(User.Identity?.Name);

        try
        {
            var result = _useCaseConfirmReservation.Execute(dto);
            return Ok(result);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPut]
    [Route("refuseReservation")]
    [Authorize(Roles = "hote")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputMyAdsAd.DtoOutputAdReservationMyAds> RefuseReservation(DtoInputAdReservationMyAds dto)
    {
        dto.userId = int.Parse(User.Identity?.Name);
        try
        {
            return Ok(_useCaseRefuseReservation.Execute(dto));
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }


    [HttpDelete]
    [Route("reservation/{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputReservation> DeleteReservation(int id)
    {
        try
        {
            return Ok(_useCaseRemoveReservation.Execute(
                new DtoInputRemoveReservation
                {
                    reservationId = id,
                    userId = Int32.Parse(User.Identity?.Name)
                }
            ));
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpGet]
    [Route("countries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<string>> FetchDistinctsCountries()
    {
        return Ok(_useCaseFetchDistinctsCountries.Execute());
    }

    [HttpGet]
    [Route("cities")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<string>> FetchDistinctsCitiesByCountry(string country)
    {
        return Ok(_useCaseFetchDistinctsCitiesByCountry.Execute(country));
    }
}
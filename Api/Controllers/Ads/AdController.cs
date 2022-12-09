using API.Utils.Picture;
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
        UseCaseFetchAllReservationByAd useCaseFetchAllReservationByAd)
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
    [Authorize(Roles = "administrateur,super-administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DtoOutputAd>> FetchAll([FromQuery] int? statusId)
    {
        return Ok(_useCaseFetchAllAds.Execute(new DtoInputFilteringAds
        {
            StatusId = statusId
        }));
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
    [Route("{id:int}/myAds")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputMyAdsAd> FetchByUserId(int id)
    {
        //Check that this is the id of the logged in user
        if ("" + id != User.Identity?.Name) return Unauthorized();

        return Ok(_useCaseFetchByUserIdAd.Execute(id));
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
    [Route("{id:int}/myReservations")]
    [Authorize(Roles = "utilisateur,hote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<DtoOutputReservation>> FetchMyReservations(int id)
    {
        //Check that this is the id of the logged in user
        if ("" + id != User.Identity?.Name) return Unauthorized();

        return Ok(_useCaseFetchMyReservations.Execute(id));
    }

    [HttpGet]
    [Route("{adSlug}/reservation")]
    [Authorize(Roles = "hote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DtoOutputMyAdReservation>> FetchAllReservationByAd(string adSlug)
    {
        return Ok(_useCaseFetchAllReservationByAd.Execute(adSlug));
    }

    [HttpGet]
    [Route("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<int> CountValidatedAds(
        [FromQuery] string? name
    )
    {
        return Ok(_useCaseCountValidatedAds.Execute(new DtoInputFilteringAds
        {
            StatusId = 3,
            Name = name
        }));
    }

    [HttpGet]
    [Route("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DtoOutputAdsSummary>> FetchForPagination(
        [FromQuery] int? limit,
        [FromQuery] int? offset,
        [FromQuery] string? name
    )
    {
        return Ok(_useCaseFetchAdsForPagination.Execute(new DtoInputFilteringAds
        {
            Limit = limit,
            Offset = offset,

            StatusId = 3,
            Name = name
        }));
    }

    [HttpPut]
    [Route("status")]
    [Authorize(Roles = "administrateur, super-administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputAd> UpdateStatus(DtoInputUpdateStatusAd dto)
    {
        return Ok(_useCaseUpdateStatusAd.Execute(dto));
    }

    [HttpPut]
    [Route("adUpdate")]
    [Authorize(Roles = "hote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputAd> UpdateAd(DtoInputUpdateAd dto) //<DtoOutputAd>
    {
        return Ok(_useCaseUpdateAd.Execute(dto));
    }

    [HttpDelete]
    [Route("{id:int}/reservation")]
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
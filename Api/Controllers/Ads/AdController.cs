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

    public AdController(UseCaseCreateAd useCaseCreateAd, UseCaseDeleteAd useCaseDeleteAd,
        UseCaseCreateReservation useCaseCreateReservation,UseCaseFetchAllAds useCaseFetchAllAds)
    {
        _useCaseCreateAd = useCaseCreateAd;
        _useCaseDeleteAd = useCaseDeleteAd;
        _useCaseCreateReservation = useCaseCreateReservation;
        _useCaseFetchAllAds = useCaseFetchAllAds;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DtoOutputAd>> FetchAll()
    {
        return Ok(_useCaseFetchAllAds.Execute());
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
}
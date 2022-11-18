using Application.UseCases.Ads;
using Application.UseCases.Ads.Dtos;

using Application.UseCases.Reservations;
using Application.UseCases.Reservations.Dtos;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Ads;

[ApiController]
[Route("api/v1/ad")]
public class AdController: ControllerBase
{
    
    private readonly UseCaseCreateAd _useCaseCreateAd;

    private readonly UseCaseDeleteAd _useCaseDeleteAd;

    private readonly UseCaseCreateReservation _useCaseCreateReservation;

    public AdController(UseCaseCreateAd useCaseCreateAd,UseCaseDeleteAd useCaseDeleteAd, UseCaseCreateReservation useCaseCreateReservation)
    {
        _useCaseCreateAd = useCaseCreateAd;
        _useCaseDeleteAd = useCaseDeleteAd;
        _useCaseCreateReservation = useCaseCreateReservation;
    }

    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputAd> Create(DtoInputCreateAd dto)
    {

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
            return Unauthorized(e.Message );
        }
    }
    
    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = "administrateur")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputAd> Delete(int id)
    {
        try
        {
            var ad = _useCaseDeleteAd.Execute(id) ;
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

    [HttpPost]
    [Route("{id:int}/reservation")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<DtoOutputReservation> CreateReservation(int id, DtoInputCreateReservation dto)
    {
        dto.AdId = id;

        return StatusCode(201, _useCaseCreateReservation.Execute(dto));

    }


}
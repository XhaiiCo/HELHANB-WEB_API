using Application.UseCases.Ads;
using Application.UseCases.Ads.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Ads;

[ApiController]
[Route("api/v1/ad")]
public class AdController: ControllerBase
{
    
    private readonly UseCaseCreateAd _useCaseCreateAd;


    public AdController(UseCaseCreateAd useCaseCreateAd)
    {
        _useCaseCreateAd = useCaseCreateAd;
    }

    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<DtoOutputAd> Create(DtoInputCreateAd dto)
    {
        return Ok(_useCaseCreateAd.Execute(dto));
    }

}
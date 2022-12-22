using Application.UseCases.Roles;
using Application.UseCases.Roles.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Roles;

[ApiController]
[Route("api/v1/roles")]
public class RoleController : ControllerBase
{
    private readonly UseCaseFetchAllRoles _useCaseFetchAllRoles;

    public RoleController(
        UseCaseFetchAllRoles useCaseFetchAllRoles
    )
    {
        _useCaseFetchAllRoles = useCaseFetchAllRoles;
    }

    [HttpGet]
    [Authorize(Roles = "administrateur,super-administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<DtoOutputRole>> FetchAll()
    {
        return Ok(_useCaseFetchAllRoles.Execute());
    }
}
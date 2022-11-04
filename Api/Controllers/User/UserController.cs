using Application.UseCases.Users;
using Application.UseCases.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.User;

[ApiController]
[Route("api/v1/users")]
public class UserController: ControllerBase
{
    private readonly UseCaseFetchAllUsers _useCaseFetchAllUsers;
    private readonly UseCaseCreateUser _useCaseCreateUser ;

    public UserController(
        UseCaseFetchAllUsers useCaseFetchAllUsers, 
        UseCaseCreateUser useCaseCreateUser)
    {
        _useCaseFetchAllUsers = useCaseFetchAllUsers;
        _useCaseCreateUser = useCaseCreateUser;
    }

    [HttpGet]
    public ActionResult<IEnumerable<DtoOutputUser>> FetchAll()
    {
        return Ok(_useCaseFetchAllUsers.Execute());
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> Create(DtoInputCreateUser userDto)
    {
        var user = _useCaseCreateUser.Execute(userDto);
        if (user != null)
            return Ok(user);
        
        return Unauthorized();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputUser> Login(DtoInputLoginUser userDto)
    {
        return Ok();
    }
}
using API.Utils.Picture;
using Application.Services.Token;
using Application.Services.User;
using Application.UseCases.Users;
using Application.UseCases.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Int32;

namespace API.Controllers.User;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _config;

    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IPictureService _pictureService;

    private readonly UseCaseFetchAllUsers _useCaseFetchAllUsers;
    private readonly UseCaseCreateUser _useCaseCreateUser;
    private readonly UseCaseLoginUser _useCaseLoginUser;
    private readonly UseCaseFetchUserById _useCaseFetchUserById;
    private readonly UseCaseDeleteUserById _useCaseDeleteUserById;
    private readonly UseCaseUpdatePasswordUser _useCaseUpdatePasswordUser;
    private readonly UseCaseUpdateUser _useCaseUpdateUser;
    private readonly UseCaseChangeRoleToHostUser _useCaseChangeRoleToHostUser;
    private readonly UseCaseChangeRole _useCaseChangeRole;
    private readonly UseCaseUpdateProfilePictureBase64 _useCaseUpdateProfilePictureBase64;
    private readonly UseCaseCountUsers _useCaseCountUsers;

    public UserController(
        UseCaseFetchAllUsers useCaseFetchAllUsers,
        UseCaseCreateUser useCaseCreateUser,
        UseCaseLoginUser useCaseLoginUser,
        ITokenService tokenService,
        IConfiguration config,
        IUserService userService,
        UseCaseFetchUserById useCaseFetchUserById,
        IPictureService pictureService,
        UseCaseDeleteUserById useCaseDeleteUserById,
        UseCaseUpdatePasswordUser useCaseUpdatePasswordUser,
        UseCaseUpdateUser useCaseUpdateUser,
        UseCaseChangeRoleToHostUser useCaseChangeRoleToHostUser,
        UseCaseChangeRole useCaseChangeRole,
        UseCaseUpdateProfilePictureBase64 useCaseUpdateProfilePictureBase64,
        UseCaseCountUsers useCaseCountUsers
    )
    {
        _useCaseFetchAllUsers = useCaseFetchAllUsers;
        _useCaseCreateUser = useCaseCreateUser;
        _useCaseLoginUser = useCaseLoginUser;
        _tokenService = tokenService;
        _config = config;
        _userService = userService;
        _useCaseFetchUserById = useCaseFetchUserById;
        _pictureService = pictureService;
        _useCaseDeleteUserById = useCaseDeleteUserById;
        _useCaseUpdatePasswordUser = useCaseUpdatePasswordUser;
        _useCaseUpdateUser = useCaseUpdateUser;
        _useCaseChangeRoleToHostUser = useCaseChangeRoleToHostUser;
        _useCaseChangeRole = useCaseChangeRole;
        _useCaseUpdateProfilePictureBase64 = useCaseUpdateProfilePictureBase64;
        _useCaseCountUsers = useCaseCountUsers;
    }

    private void AppendCookies(string token)
    {
        //Create the cookie options
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true
        };

        Response.Cookies.Append("jwt", token, cookieOptions);
    }

    private bool IsTheIdOfConnectedUser(int id)
    {
        return "" + id == User.Identity?.Name;
    }

    private string GenerateToken(DtoTokenUser tokenUser)
    {
        return _tokenService.BuildToken(_config["Jwt:Key"], _config["Jwt:Issuer"], tokenUser);
    }

    [HttpGet]
    [Route("count")]
    [Authorize(Roles = "administrateur,super-administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<int> CountUsers([FromQuery] string? role, [FromQuery] string? search)
    {
        return Ok(_useCaseCountUsers.Execute(new DtoInputFilteringUsers
        {
            Role = role,
            Search = search
        }));
    }

    [HttpGet]
    [Authorize(Roles = "administrateur,super-administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<DtoOutputUser>> FetchAll(
        [FromQuery] int? limit,
        [FromQuery] int? offset,
        [FromQuery] string? role,
        [FromQuery] string? search)
    {
        return Ok(_useCaseFetchAllUsers.Execute(new DtoInputFilteringUsers
        {
            Limit = limit,
            Offset = offset,
            Role = role,
            Search = search
        }));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "administrateur,super-administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoOutputUser> Delete(int id)
    {
        if (IsTheIdOfConnectedUser(id)) return Unauthorized("Vous ne pouvez pas vous supprimer");

        var userToDelete = _userService.FetchById(id);
        var userWhoMakeRequest = _userService.FetchById(int.Parse(User.Identity?.Name));
        if (userWhoMakeRequest.Role.Name == "administrateur")
            if (userToDelete.Role.Name is "administrateur" or "super-administrateur")
                return Unauthorized("Vous ne pouvez pas supprimer cet utilisateur");

        //Remove the current profile picture if exist
        if (userToDelete.ProfilePicturePath != null)
        {
            _pictureService.RemoveFile(userToDelete.ProfilePicturePath);
        }

        try
        {
            var user = _useCaseDeleteUserById.Execute(id);
            return Ok(user);
        }
        catch (KeyNotFoundException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [Route("connected")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> IsConnected()
    {
        var userId = User.Identity?.Name;
        if (userId == null) return Unauthorized();

        return Ok(_useCaseFetchUserById.Execute(Parse(userId)));
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("registration")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoOutputUser> Create(DtoInputCreateUser userDto)
    {
        try
        {
            var user = _useCaseCreateUser.Execute(userDto);
            if (user != null)
            {
                //Login the user
                var tokenUser = new DtoTokenUser
                {
                    Id = user.Id,
                    RoleName = user.Role.Name
                };
                var generatedToken = this.GenerateToken(tokenUser);
                AppendCookies(generatedToken);

                return Ok(user);
            }
        }
        catch (Exception e)
        {
            return Conflict(e.Message);
        }

        return Unauthorized();
    }

    [HttpPut]
    [Authorize]
    [Route("password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> UpdatePassword(DtoInputUpdatePasswordUser dto)
    {
        //Check that this is the id of the logged in user
        if (!IsTheIdOfConnectedUser(dto.Id)) return Unauthorized();

        return Ok(_useCaseUpdatePasswordUser.Execute(dto));
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> UpdateUser(DtoInputUpdateUser dtoInputUpdateUser)
    {
        //Check that this is the id of the logged in user
        if (!IsTheIdOfConnectedUser(dtoInputUpdateUser.Id)) return Unauthorized();

        try
        {
            var user = _useCaseUpdateUser.Execute(dtoInputUpdateUser);

            return Ok(user);
        }
        catch (Exception e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut]
    [Authorize]
    [Route("profilePicture/base64")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> UpdateProfilePictureBase64(DtoInputProfilePictureBase64 dto)
    {
        dto.userId = int.Parse(User.Identity?.Name);

        return Ok(_useCaseUpdateProfilePictureBase64.Execute(dto));
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputUserLogin> Login(DtoInputLoginUser userDto)
    {
        try
        {
            var user = _useCaseLoginUser.Execute(userDto);

            //Login the user
            var tokenUser = new DtoTokenUser
            {
                Id = user.Id,
                RoleName = user.Role.Name
            };

            var generatedToken = this.GenerateToken(tokenUser);

            AppendCookies(generatedToken);

            return Ok(user);
        }
        catch (KeyNotFoundException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [Route("disconnect")]
    public void Disconnect()
    {
        Response.Cookies.Delete("jwt");
    }

    [HttpPut]
    [Authorize(Roles = "utilisateur")]
    [Route("{id:int}/becomeHost")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> BecomeHost(int id)
    {
        //Check that this is the id of the logged in user
        if (!IsTheIdOfConnectedUser(id)) return Unauthorized();

        try
        {
            var user = _useCaseChangeRoleToHostUser.Execute(id);

            //Change the cookie with the role
            var tokenUser = new DtoTokenUser
            {
                Id = user.Id,
                RoleName = user.Role.Name
            };

            var generatedToken = this.GenerateToken(tokenUser);
            AppendCookies(generatedToken);

            return Ok(user);
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
    }

    [HttpPut]
    [Authorize(Roles = "super-administrateur")]
    [Route("{id:int}/changeRole/{newRoleId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public ActionResult<DtoOutputUser> ChangeRole(int id, int newRoleId)
    {
        try
        {
            var userNewRole = new DtoUserNewRole
            {
                Id = id,
                RoleId = newRoleId
            };

            var result = _useCaseChangeRole.Execute(userNewRole);
            return Ok(result);
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
    }
}
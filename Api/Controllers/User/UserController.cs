using API.Utils.Picture;
using Application.Services.Token;
using Application.Services.User;
using Application.UseCases.Roles;
using Application.UseCases.Users;
using Application.UseCases.Users.Dtos;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Int32;

namespace API.Controllers.User;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    public static IWebHostEnvironment _environment;
    private IConfiguration _config;

    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IPictureService _pictureService;

    private readonly UseCaseFetchAllUsers _useCaseFetchAllUsers;
    private readonly UseCaseCreateUser _useCaseCreateUser;
    private readonly UseCaseLoginUser _useCaseLoginUser;
    private readonly UseCaseUpdateUserProfilePicture _useCaseUpdateUserProfilePicture;
    private readonly UseCaseFetchUserById _useCaseFetchUserById;
    private readonly UseCaseDeleteUserById _useCaseDeleteUserById;
    private readonly UseCaseUpdatePasswordUser _useCaseUpdatePasswordUser;
    private readonly UseCaseUpdateUser _useCaseUpdateUser;
    private readonly UseCaseChangeRoleToHostUser _useCaseChangeRoleToHostUser;
    private readonly UseCaseChangeRole _useCaseChangeRole;

    public UserController(
        UseCaseFetchAllUsers useCaseFetchAllUsers,
        UseCaseCreateUser useCaseCreateUser,
        UseCaseLoginUser useCaseLoginUser,
        ITokenService tokenService,
        IConfiguration config,
        IWebHostEnvironment environment,
        IUserService userService,
        UseCaseUpdateUserProfilePicture useCaseUpdateUserProfilePicture,
        UseCaseFetchUserById useCaseFetchUserById,
        IPictureService pictureService,
        UseCaseDeleteUserById useCaseDeleteUserById,
        UseCaseUpdatePasswordUser useCaseUpdatePasswordUser,
        UseCaseUpdateUser useCaseUpdateUser, UseCaseChangeRoleToHostUser useCaseChangeRoleToHostUser,
        UseCaseChangeRole useCaseChangeRole)
    {
        _useCaseFetchAllUsers = useCaseFetchAllUsers;
        _useCaseCreateUser = useCaseCreateUser;
        _useCaseLoginUser = useCaseLoginUser;
        _tokenService = tokenService;
        _config = config;
        _environment = environment;
        _userService = userService;
        _useCaseUpdateUserProfilePicture = useCaseUpdateUserProfilePicture;
        _useCaseFetchUserById = useCaseFetchUserById;
        _pictureService = pictureService;
        _useCaseDeleteUserById = useCaseDeleteUserById;
        _useCaseUpdatePasswordUser = useCaseUpdatePasswordUser;
        _useCaseUpdateUser = useCaseUpdateUser;
        _useCaseChangeRoleToHostUser = useCaseChangeRoleToHostUser;
        _useCaseChangeRole = useCaseChangeRole;
    }

    private void AppendCookies(string token)
    {
        //Create the cookie options
        CookieOptions cookieOptions = new CookieOptions
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
    [Authorize(Roles = "administrateur,super-administrateur")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<DtoOutputUser>> FetchAll([FromQuery] string? role, [FromQuery] string? search)
    {
        return Ok(_useCaseFetchAllUsers.Execute(new DtoInputFilteringUsers
        {
            Role = role,
            Search = search
        }));
    }

    [HttpDelete]
    [Route("{id}")]
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
                DtoTokenUser tokenUser = new DtoTokenUser
                {
                    Id = user.Id,
                    RoleName = user.Role.Name
                };
                var generatedToken = this.GenerateToken(tokenUser);
                this.AppendCookies(generatedToken);

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
    [Route("profilePicture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> UpdateProfilePicture(IFormFile? profilePicture)
    {
        try
        {
            var id = int.Parse(User.Identity?.Name);
            var currentUser = _userService.FetchById(id);

            //If the protilePicture is null remove it
            if (profilePicture == null)
            {
                //Remove the current profile picture if exist
                if (currentUser.ProfilePicturePath != null && currentUser.ProfilePicturePath !=
                    "\\Upload\\ProfilePicture\\default_user_pic.png")
                {
                    _pictureService.RemoveFile(currentUser.ProfilePicturePath);
                }

                var dtoInputUpdateProfilePictureUser = new DtoInputUpdateProfilePictureUser
                {
                    Id = id,
                    ProfilePicturePath = "\\Upload\\ProfilePicture\\default_user_pic.png"
                };
                var user = _useCaseUpdateUserProfilePicture.Execute(dtoInputUpdateProfilePictureUser);

                return Ok(user);
            }

            //Else add the  new profile picture
            else if (profilePicture.Length > 0)
            {
                var basePath = "\\Upload\\ProfilePicture\\";

                //Check the file type
                if (!_pictureService.ValidPictureType(profilePicture.ContentType))
                {
                    return Unauthorized("Extension d'image invalide acceptés: jpeg, png");
                }

                //Create a unique file name
                var fileName = _pictureService.GenerateUniqueFileName(id) + profilePicture.FileName;


                //Remove the current profile picture if exist
                if (currentUser.ProfilePicturePath != null)
                {
                    _pictureService.RemoveFile(currentUser.ProfilePicturePath);
                }

                //Update the user
                var dtoInputUpdateProfilePictureUser = new DtoInputUpdateProfilePictureUser
                {
                    Id = id,
                    ProfilePicturePath = basePath + fileName
                };
                var user = _useCaseUpdateUserProfilePicture.Execute(dtoInputUpdateProfilePictureUser);

                //Upload the new picture
                this._pictureService.UploadPicture(basePath, fileName, profilePicture);
                return Ok(user);
            }
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }

        return Unauthorized("Failed");
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputUserToken> Login(DtoInputLoginUser userDto)
    {
        try
        {
            var user = _useCaseLoginUser.Execute(userDto);

            //Login the user
            DtoTokenUser tokenUser = new DtoTokenUser
            {
                Id = user.Id,
                RoleName = user.Role.Name
            };

            var generatedToken = this.GenerateToken(tokenUser);
            user.Token = generatedToken;

            this.AppendCookies(generatedToken);

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
            DtoTokenUser tokenUser = new DtoTokenUser
            {
                Id = user.Id,
                RoleName = user.Role.Name
            };

            var generatedToken = this.GenerateToken(tokenUser);
            this.AppendCookies(generatedToken);

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
            DtoUserNewRole userNewRole = new DtoUserNewRole
            {
                Id = id,
                RoleId = newRoleId
            };

            return Ok(_useCaseChangeRole.Execute(userNewRole));
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
    }
}
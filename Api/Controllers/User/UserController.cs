using Application;
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
    public static IWebHostEnvironment _environment;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private IConfiguration _config;

    private readonly UseCaseFetchAllUsers _useCaseFetchAllUsers;
    private readonly UseCaseCreateUser _useCaseCreateUser;
    private readonly UseCaseLoginUser _useCaseLoginUser;
    private readonly UseCaseUpdateUserProfilePicture _useCaseUpdateUserProfilePicture;
    private readonly UseCaseFetchUserById _useCaseFetchUserById;

    public UserController(
        UseCaseFetchAllUsers useCaseFetchAllUsers,
        UseCaseCreateUser useCaseCreateUser,
        UseCaseLoginUser useCaseLoginUser,
        ITokenService tokenService,
        IConfiguration config,
        IWebHostEnvironment environment,
        IUserService userService,
        UseCaseUpdateUserProfilePicture useCaseUpdateUserProfilePicture,
        UseCaseFetchUserById useCaseFetchUserById)
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

    private string GenerateToken(DtoTokenUser tokenUser)
    {
        return _tokenService.BuildToken(_config["Jwt:Key"], _config["Jwt:Issuer"], tokenUser);
    }

    [HttpGet]
//    [Authorize(Roles = "2")]
    public ActionResult<IEnumerable<DtoOutputUser>> FetchAll()
    {
        return Ok(_useCaseFetchAllUsers.Execute());
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
                DtoTokenUser tokenUser = Mapper.GetInstance().Map<DtoTokenUser>(user);
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

    [HttpPost]
    [Authorize]
    [Route("{id}/profilePicture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> UpdateProfilePicture(int id, IFormFile profilePicture)
    {
        //Check that this is the id of the logged in user
        if ("" + id != User.Identity?.Name) return Unauthorized();

        try
        {
            if (profilePicture.Length > 0)
            {
                var basePath = "\\Upload\\ProfilePicture\\";

                //Check the file type
                string[] fileTypes = { "image/jpeg", "image/png" };
                if (!fileTypes.Contains(profilePicture.ContentType)) return Unauthorized("File type invalid");

                //Create a unique file name
                var fileName = id + "_" + DateTime.Now.Ticks + "_" + profilePicture.FileName.Replace(" ", "");

                //Create the directory
                if (!Directory.Exists(_environment.WebRootPath + basePath))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + basePath);
                }

                var currentUser = _userService.FetchById(id);
                if (currentUser.ProfilePicturePath != null)
                {
                    //TODO: remove the current user profile picture 
                }

                var dtoInputUpdateProfilePictureUser = new DtoInputUpdateProfilePictureUser
                {
                    Id = id,
                    ProfilePicturePath = basePath + fileName
                };
                var user = _useCaseUpdateUserProfilePicture.Execute(dtoInputUpdateProfilePictureUser);

                using var fileStream = System.IO.File.Create(_environment.WebRootPath +
                                                             basePath +
                                                             fileName);
                //Copy the file to the directory
                profilePicture.CopyTo(fileStream);
                fileStream.Flush();
                return Ok(user);
            }
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message.ToString());
        }

        return Unauthorized("Failed");
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DtoOutputUser> Login(DtoInputLoginUser userDto)
    {
        try
        {
            var user = _useCaseLoginUser.Execute(userDto);

            //Login the user
            DtoTokenUser tokenUser = Mapper.GetInstance().Map<DtoTokenUser>(user);
            var generatedToken = this.GenerateToken(tokenUser);
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
}
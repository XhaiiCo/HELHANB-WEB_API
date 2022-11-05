using Application;
using Application.Services.Token;
using Application.Services.User;
using Application.UseCases.Users;
using Application.UseCases.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.User;

[ApiController]
[Route("api/v1/users")]
public class UserController: ControllerBase
{
    public static IWebHostEnvironment _environment;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private IConfiguration _config;  
    
    private readonly UseCaseFetchAllUsers _useCaseFetchAllUsers;
    private readonly UseCaseCreateUser _useCaseCreateUser ;
    private readonly UseCaseLoginUser _useCaseLoginUser;
    private readonly UseCaseUpdateUserProfilePicture _useCaseUpdateUserProfilePicture;

    public UserController(
        UseCaseFetchAllUsers useCaseFetchAllUsers, 
        UseCaseCreateUser useCaseCreateUser,
        UseCaseLoginUser useCaseLoginUser,
        ITokenService tokenService,
        IConfiguration config,
        IWebHostEnvironment environment,
        IUserService userService,
        UseCaseUpdateUserProfilePicture useCaseUpdateUserProfilePicture)
    {
        _useCaseFetchAllUsers = useCaseFetchAllUsers;
        _useCaseCreateUser = useCaseCreateUser;
        _useCaseLoginUser = useCaseLoginUser;
        _tokenService = tokenService;
        _config = config;
        _environment = environment;
        _userService = userService;
        _useCaseUpdateUserProfilePicture = useCaseUpdateUserProfilePicture;
    }

    [HttpGet]
    [Authorize(Roles = "2")]
    public ActionResult<IEnumerable<DtoOutputUser>> FetchAll()
    {
        return Ok(_useCaseFetchAllUsers.Execute());
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("registration")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> Create(DtoInputCreateUser userDto)
    {
        var user = _useCaseCreateUser.Execute(userDto);

        if (user != null)
        {
            //Login the user
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true
            };
            DtoTokenUser tokenUser = Mapper.GetInstance().Map<DtoTokenUser>(user);
            string generatedToken = _tokenService.BuildToken(_config["Jwt:Key"], _config["Jwt:Issuer"], tokenUser);
            Response.Cookies.Append("jwt", generatedToken, cookieOptions); 
            
            return Ok(user);
        }
        
        return Unauthorized();
    }

    [HttpPost]
    [Route("{id}/profilePicture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> UpdateProfilePicture(int id, IFormFile profilePicture)
    {
        try
        {
            if (profilePicture.Length > 0)
            {
                var basePath = "\\Upload\\ProfilePicture\\";
                
                //Check the file type
                string[] fileTypes = { "image/jpeg", "image/png" };
                if (!fileTypes.Contains(profilePicture.ContentType)) return Unauthorized("File type invalid");
                
                //Create a unique file name
                var fileName = id + "_" + DateTime.Now.Ticks + "_" +  profilePicture.FileName;
                
                //Create the directory
                if (!Directory.Exists(_environment.WebRootPath + basePath))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + basePath);
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
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true
            };
            DtoTokenUser tokenUser = Mapper.GetInstance().Map<DtoTokenUser>(user);
            string generatedToken = _tokenService.BuildToken(_config["Jwt:Key"], _config["Jwt:Issuer"], tokenUser);
            Response.Cookies.Append("jwt", generatedToken, cookieOptions); 

            return Ok(user);
        }
        catch (KeyNotFoundException e)
        {
            return Unauthorized(e.Message);
        }
    }
}
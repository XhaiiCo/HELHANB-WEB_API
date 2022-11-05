using Application;
using Application.Services.Token;
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
    private IConfiguration _config;  
    
    private readonly UseCaseFetchAllUsers _useCaseFetchAllUsers;
    private readonly UseCaseCreateUser _useCaseCreateUser ;
    private readonly UseCaseLoginUser _useCaseLoginUser;

    public UserController(
        UseCaseFetchAllUsers useCaseFetchAllUsers, 
        UseCaseCreateUser useCaseCreateUser,
        UseCaseLoginUser useCaseLoginUser,
        ITokenService tokenService,
        IConfiguration config,
        IWebHostEnvironment environment)
    {
        _useCaseFetchAllUsers = useCaseFetchAllUsers;
        _useCaseCreateUser = useCaseCreateUser;
        _useCaseLoginUser = useCaseLoginUser;
        _tokenService = tokenService;
        _config = config;
        _environment = environment;
    }

    [HttpGet]
    [Authorize(Roles = "2")]
    public ActionResult<IEnumerable<DtoOutputUser>> FetchAll()
    {
        return Ok(_useCaseFetchAllUsers.Execute());
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputUser> Create(DtoInputCreateUser userDto)
    {
        var user = _useCaseCreateUser.Execute(userDto);

        if (user != null)
        {
            return Ok(user);
        }
        
        return Unauthorized();
    }

    [HttpPost]
    [Route("{id}/profilPicture")]
    public string Post(int id, IFormFile profilePicture)
    {
        
        try
        {
            if (profilePicture.Length > 0)
            {
                var basePath = "\\Upload\\ProfilePicture\\";
                
                //Check the file type
                string[] fileTypes = { "image/jpeg", "image/png" };
                if (!fileTypes.Contains(profilePicture.ContentType)) return "File type invalid";
                
                //Create a unique file name
                var fileName = id + "_" + DateTime.Now.Ticks + "_" +  profilePicture.FileName;
                
                //Create the directory
                if (!Directory.Exists(_environment.WebRootPath + basePath))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + basePath);
                }

                using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath +
                                                                     basePath +
                                                                     fileName))
                {
                    profilePicture.CopyTo(fileStream);
                    fileStream.Flush();
                    return basePath + fileName;
                }
            }
            else
            {
                return "Failed";
            }
        }
        catch (Exception e)
        {
            return e.Message.ToString();
        }
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
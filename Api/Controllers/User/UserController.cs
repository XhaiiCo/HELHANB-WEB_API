using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.User;

[ApiController]
[Route("api/v1/users")]
public class UserController: ControllerBase
{
    [HttpGet]
    public ActionResult FetchAll()
    {
        return Ok();
    }

}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.ChatController;

[Route("api/chat")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [Route("send")]                                           //path looks like this: https://localhost:44379/api/chat/send
    [HttpPost]
    public IActionResult SendRequest([FromBody] MessageDto msg)
    {
        _hubContext.Clients.All.SendAsync("ReceiveOne", msg.user, msg.msgText);
        return Ok();
    }
}
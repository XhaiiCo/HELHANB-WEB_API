using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.ChatController;

[Authorize]
public class ChatHub : Hub // inherit this
{
    public override Task OnConnectedAsync()
    { 
        Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
        return base.OnConnectedAsync();
    }
    
    public Task SendMessage1(string user, string message, string group) // Two parameters accepted
    {
        return Clients.Groups(Context.User.Identity.Name).SendAsync("ReceiveOne", user, message); // Note this 'ReceiveOne' 
    }
}
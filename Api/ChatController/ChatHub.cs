using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.ChatController;

public class ChatHub : Hub // inherit this
{
    [Authorize]
    public override Task OnConnectedAsync()
    {
        Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
        return base.OnConnectedAsync();
    }

    [Authorize]
    public Task SendMessage1(string message, int recipientId, int senderId)
    {
        return Clients.Group("" + recipientId).SendAsync("ReceiveOne", message, senderId);
    }

    [Authorize]
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
        return base.OnDisconnectedAsync(exception);
    }
}
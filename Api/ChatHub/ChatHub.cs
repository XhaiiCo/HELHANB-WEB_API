using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.ChatController;

public class ChatHub : Hub
{
    [Authorize]
    public override Task OnConnectedAsync()
    {
        //When a user logis in to the chat, he is added to a group that has his ID as its name, for contact them individually later.
        Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
        return base.OnConnectedAsync();
    }

    /// <summary>
    /// is a method that sends a message to a specific user
    /// </summary>
    /// <param name="message">The message to be sent</param>
    /// <param name="recipientId">The user id of the recipient</param>
    /// <param name="senderId">The id of the user who sent the message</param>
    /// <returns>
    /// A Task
    /// </returns>
    [Authorize]
    public Task SendMessage1(string message, int recipientId, int senderId)
    {
        return Clients.Group("" + recipientId).SendAsync("ReceiveOne", message, senderId);
    }

    [Authorize]
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        //When the user logs out, they are removed from the group  
        Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
        return base.OnDisconnectedAsync(exception);
    }
}
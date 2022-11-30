using Microsoft.AspNetCore.SignalR;

namespace API.ChatController;

public class ChatHub : Hub // inherit this
{
    public Task SendMessage1(string user, string message, string group) // Two parameters accepted
    {
        Groups.AddToGroupAsync(Context.ConnectionId, group);
        return Clients.Groups(group).SendAsync("ReceiveOne", user, message); // Note this 'ReceiveOne' 
    }
}
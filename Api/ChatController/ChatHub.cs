using Microsoft.AspNetCore.SignalR;

namespace API.ChatController;

public class ChatHub : Hub                                              // inherit this
{
    
    //work without it
    
    public Task SendMessage1(string user, string message)               // Two parameters accepted
    {
        return Clients.All.SendAsync("ReceiveOne", user, message);    // Note this 'ReceiveOne' 
    }
}
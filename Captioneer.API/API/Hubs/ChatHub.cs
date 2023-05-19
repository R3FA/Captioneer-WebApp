using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendToUser(string loggedUsername,string userConnectionId, string message)
        {
            await Clients.Client(userConnectionId).SendAsync("ReceiveMessage", loggedUsername, message);
        }
        public string GetConnectionID() => Context.ConnectionId;
    }
}
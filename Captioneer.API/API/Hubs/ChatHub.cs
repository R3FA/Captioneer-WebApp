using API.Data;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> userDictionary = new Dictionary<string, string>();
        public async Task SendToUser(string loggedUsername,string friendUsername, string message)
        {
            var connectionID = userDictionary[friendUsername];
            await Clients.Client(connectionID).SendAsync("ReceiveMessage", loggedUsername, message);
        }
        public string GetConnectionID(string loggedUser)
        {
            if (userDictionary.ContainsKey(loggedUser))
            {
                userDictionary.Remove(loggedUser);
            }
            userDictionary.Add(loggedUser, Context.ConnectionId);
            return userDictionary[loggedUser];
        }
    }
}
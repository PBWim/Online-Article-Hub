namespace Web.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    // SignalR
    // https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-2.2&tabs=visual-studio
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
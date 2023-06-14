using Microsoft.AspNetCore.SignalR;
using EShopOnlineExam.Models;

namespace EShopOnlineExam.Hubs
{
    public class ChatHub :Hub
    {
        public async Task SendMessage(Messages message)
        {
            await Clients.All.SendAsync("receiveMessage", message);

        }
    }
}

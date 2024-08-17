using chat.Server.models;
using Microsoft.AspNetCore.SignalR;

namespace chat.Server.services
{
    public class ChatHub : Hub
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ChatHub(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task SendMessage(string msg)
        {
            //if (string.IsNullOrEmpty(message.User.UserName) || string.IsNullOrEmpty("message"))
            //{
            //    return;
            //}

            //var messageObj = new Message() {
            //    Value = message.Value
            //};

            //var client = httpClientFactory.CreateClient();
            //await client.PostAsJsonAsync("http://localhost:7282/api/chat", msg);

            await Clients.All.SendAsync("ReceiveMessage", msg);
            //await Clients.Clients(this.Context.ConnectionId).SendAsync("ReceiveMessage", message);
        }
    }
}

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
        public async Task SendMessage(string userName, string message)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty("message"))
            {
                return;
            }

            var messageObj = new Message() {
                Value = message
            };

            var client = httpClientFactory.CreateClient();
            await client.PostAsJsonAsync("http://localhost:7282/api/chat", messageObj);

            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }
    }
}

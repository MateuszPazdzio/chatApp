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
        public async Task SendMessage(SendingMessage message)
        {
            if (string.IsNullOrEmpty(message.User.UserName) || string.IsNullOrEmpty("message"))
            {
                return;
            }

            //var messageObj = new Message() {
            //    Value = message.Value
            //};

            var client = httpClientFactory.CreateClient();
            try
            {

                await client.PostAsJsonAsync("https://localhost:7282/api/chat/message", message);
            }
            catch (Exception)
            {

                throw;
            }

            //await Clients.All.SendAsync("ReceiveMessage", message);
            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", message);
            //await Clients.Clients(this.Context.ConnectionId).SendAsync("ReceiveMessage", message);
        }

        public async Task JoinChat(string chatId)
        {
            // Add the current connection to the chat group
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

            // Optionally, notify others that this user has joined
            await Clients.Group(chatId).SendAsync("UserJoined", Context.ConnectionId);
        }

        public async Task LeaveChat(string chatId)
        {
            // Remove the current connection from the chat group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);

            // Optionally, notify others that this user has left
            await Clients.Group(chatId).SendAsync("UserLeft", Context.ConnectionId);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Handle disconnection: remove the user from all chat groups they are part of
            // You might want to keep track of which groups the user is part of
            // Here we're assuming you know the chatId or have stored it somewhere
            //var chatId = ...; // Retrieve the chatId for the disconnected user
            //if (chatId != null)
            //{
            //    await LeaveChat(chatId);
            //}

            await base.OnDisconnectedAsync(exception);
        }
    }
}

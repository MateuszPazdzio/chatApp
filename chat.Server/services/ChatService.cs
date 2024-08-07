using chat.Server.Data;
using chat.Server.models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace chat.Server.services
{
    public interface IChatService
    {
        Task<Chat> GetChat(string userName);

        //Task<string> GetChatName(int id);
        Task<IEnumerable<Message>> GetChatMessages(int id);
    }
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _chatDbContext;

        public ChatService(ChatDbContext chatDbContext)
        {
            this._chatDbContext = chatDbContext;
        }

        public Task<Chat> GetChat(string userName)
        {
           var chat =_chatDbContext.Chats.FirstOrDefaultAsync(c=>c.Name==userName);
            if (chat == null) throw new Exception("Chat does not exists");
            return chat;

        }

        public Task<IEnumerable<Message>> GetChatMessages(int id)
        {
            throw new Exception("");
        }
    }
}

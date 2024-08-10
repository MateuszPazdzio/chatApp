using chat.Server.Data;
using chat.Server.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace chat.Server.services
{
    public interface IChatService
    {
        Task<bool> AddMessage(Message message);
        Task<Chat> GetChat(string userName);

        //Task<string> GetChatName(int id);
        IEnumerable<Message> GetChatMessages(SearchResult id);
        Task<List<SearchResult>> GetProposedChatConversationsBySearchInput(string userName);
    }
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _chatDbContext;

        public ChatService(ChatDbContext chatDbContext)
        {
            this._chatDbContext = chatDbContext;
        }

        public Task<bool> AddMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<Chat> GetChat(string userName)
        {
           var chat =_chatDbContext.Chats.FirstOrDefaultAsync(c=>c.Name==userName);
            if (chat == null) throw new Exception("Chat does not exists");
            return chat;

        }

        public IEnumerable<Message> GetChatMessages(SearchResult searchResult)
        {
            List<string> usersEmails = new List<string>();
            usersEmails= searchResult.Users.Select(u=>u.Email).ToList();
            var parameterList =new List<SqlParameter>();


            var emailTable = new DataTable();
            usersEmails.Add("string");
            emailTable.Columns.Add("Value", typeof(string));

            foreach (var email in usersEmails)
            {
                emailTable.Rows.Add(email);
            }


            var emailParameter = new SqlParameter("@EmailList", SqlDbType.Structured)
            {
                TypeName = "dbo.EmailList",
                Value = emailTable
            };

            var chatNameParameter = new SqlParameter("@ChatName", searchResult.Name);
            parameterList.Add(emailParameter);
            parameterList.Add(chatNameParameter);
            IEnumerable<Message> messages;
            try
            {
                //messages =  _chatDbContext.Database.SqlQueryRaw<Message>("EXEC GetMessagesFromChat @EmailList, @ChatName ",
                //    parameterList.ToArray()
                //);
                messages = _chatDbContext.Database.SqlQuery<Message>($"EXEC GetMessagesFromChat {emailParameter}, {chatNameParameter}").ToList();

            }
            catch (Exception)
            {
                throw;
            }

            return messages;
        }

        public async Task<List<SearchResult>> GetProposedChatConversationsBySearchInput(string searchPhrase)
        {
            var users = await _chatDbContext.Users.Where(u=>u.UserName.Contains(searchPhrase)).ToListAsync();

            var searchResults = new List<SearchResult>();

            foreach (var user in users)
            {
                var userFrom1To1Chat =new List<User>();
                userFrom1To1Chat.Add(user);
                var searchResult = new SearchResult()
                {
                    Users = userFrom1To1Chat,
                    Name = user.UserName,
                };

                searchResults.Add(searchResult);
            }

            return searchResults;

        }

    }
}

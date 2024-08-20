using chat.Server.Data;
using chat.Server.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace chat.Server.services
{
    public interface IChatService
    {
        Task<bool> AddMessage(SendingMessage message);
        Task<int> CreateChat(Chat chat);
        Task<Chat> GetChat(string userName);

        //Task<string> GetChatName(int id);
        ChatDetails GetMessagesFromChatAndChatId(SearchResult id);
        Task<List<SearchResult>> GetProposedChatConversationsBySearchInput(string userName);
    }
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _chatDbContext;

        public ChatService(ChatDbContext chatDbContext)
        {
            this._chatDbContext = chatDbContext;
        }

        public async Task<bool> AddMessage(SendingMessage message)
        {
            if (message.ChatId == 0)
            {
                throw new Exception("chat does not exists.");
            }
            try
            {
                var user=await _chatDbContext.Users.FirstOrDefaultAsync(x => x.UserName == message.User.UserName);
                if (user == null)
                {
                    throw new Exception("User does not exist.");
                }

                // Create the new message entity
                Message newMsg = new Message()
                {
                    ChatId = message.ChatId,
                    SendingDate = message.SendingDate,
                    User = user, // Set the user directly here
                    Value = message.Value,
                };
                await _chatDbContext.Messages.AddAsync(newMsg);
                await _chatDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public async Task<int> CreateChat(Chat chat)
        {
            Chat chatExists = await CheckIfChatExists(chat);
            if(chatExists!=null)
            {
                return chatExists.Id;
            }
            chat.CreationDate = DateTime.Now;
            try
            {
                // Ensure users are attached to the context and won't be reinserted
                var existingUsers = new List<User>();
                foreach (var user in chat.Users)
                {
                    var existingUser = await _chatDbContext.Users.FirstOrDefaultAsync(u=>u.UserName==user.UserName);
                    if (existingUser != null)
                    {
                        existingUsers.Add(existingUser);
                    }
                    else
                    {
                        // Handle case where user doesn't exist, e.g., log error or return false
                        return 0;
                    }
                }

                // Clear chat users to prevent EF from attempting to add them again
                chat.Users.Clear();

                // Add the new chat to the context
                var chatAdded=await _chatDbContext.Chats.AddAsync(chat);

                // Associate existing users with the chat
                chat.Users.AddRange(existingUsers);

                // Save changes to the database
                await _chatDbContext.SaveChangesAsync();

                return chat.Id;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return 0;
            }
        }

        private async Task<Chat> CheckIfChatExists(Chat chat)
        {

            try
            {
                var chatWithUsers = await _chatDbContext.Chats
                    .Include(c => c.Users)
                    .Where(c => c.Name == chat.Name).ToListAsync();

                // If chat is found, perform client-side filtering for users
                foreach (Chat chatItem  in chatWithUsers)
                {
                    //chat.Users.All(user => chatItem.Users.Any(u => u.UserName == user.UserName))){
                    //    return true;
                    //}

                    Chat c= chatWithUsers.FirstOrDefault(d=>chat.Users.All(user => chatItem.Users.Any(u => u.UserName == user.UserName)));
                    if (c!=null && c.Id != 0)
                    {
                        return c;
                    }
                    //foreach (User user in chat.Users){
                    //{
                    //    if (!chatItem.Users.Any(u => u.UserName == user.UserName))
                    //    {
                    //        break;
                    //    }
                    //}
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public Task<Chat> GetChat(string userName)
        {
           var chat =_chatDbContext.Chats.FirstOrDefaultAsync(c=>c.Name==userName);
            if (chat == null) throw new Exception("Chat does not exists");
            return chat;

        }

        public  ChatDetails GetMessagesFromChatAndChatId(SearchResult searchResult)
        {
            List<string> usersEmails = new List<string>();
            usersEmails= searchResult.Users.Select(u=>u.Email).ToList();
            var parameterList =new List<SqlParameter>();


            var emailTable = new DataTable();
            //usersEmails.Add("string");
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

            var chatNameParameter = new SqlParameter("@ChatName",  (object)DBNull.Value );
            //var chatNameParameter = new SqlParameter("@ChatName",  (object)DBNull.Value);
            parameterList.Add(emailParameter);
            parameterList.Add(chatNameParameter);
            IEnumerable<MessageDto> response;
            ChatDetails chatDetails;
            try
            {
                //messages =  _chatDbContext.Database.SqlQueryRaw<Message>("EXEC GetMessagesFromChat @EmailList, @ChatName ",
                //    parameterList.ToArray()
                //);
                response = _chatDbContext.Database.SqlQuery<MessageDto>($"EXEC GetMessagesFromChat {emailParameter}, {chatNameParameter}").AsEnumerable()
                    .ToList();
                if (response.Count() == 0) {
                    return new ChatDetails();
                }
                chatDetails = new ChatDetails()
                {
                    ChatId = response.FirstOrDefault().ChatId,
                    Messages = response.Select(reader => new Message()
                    {

                        ChatId = reader.ChatId,
                        SendingDate = reader.SendingDate,
                        Value = reader.Value,
                        User = new User()
                        {
                            Email = reader.Email,
                            UserName = reader.UserName,
                        }
                    }
                   ).ToList()
                };
            }
            catch (Exception)
            {
                return new ChatDetails();
            }

            return chatDetails;
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

using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace chat.Server.models
{
    public class User : IdentityUser
    { 
        public int Id { get; set; }
        public override string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public List<Chat> Chats { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Server.models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Value { get; set; }
        [NotMapped]
        public User User { get; set; }
        public DateTime? SendingDate { get; set; }
        //public DateTime Time { get; set; }
    }
    public class SendingMessage : Message
    {
        public int ChatId { get; set; }
    }
    public class MessageDto
    {
        public int ChatId { get; set; }
        public string Value { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime SendingDate { get; set; }
    }
}
namespace chat.Server.models
{
    public class ChatDetails
    {
        public int ChatId { get; set; }
        public List<Message> Messages { get; set; }
    }
}
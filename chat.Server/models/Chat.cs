namespace chat.Server.models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
    }
}
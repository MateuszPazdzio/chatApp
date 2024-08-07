namespace chat.Server.models.dtos
{
    public class ChatDto
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public List<User> Users { get; set; }
    }
}

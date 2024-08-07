namespace chat.Server.models
{
    public interface IUserHttpContext
    {
        public CurrentUser GetUser();
    }
}

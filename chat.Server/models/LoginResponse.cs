namespace chat.Server.models
{
    public class LoginResponse
    {
        public bool IsLoggedIn { get; set; }
        public string ResponseMessage { get; set; }
        public string Token { get; set; }
    }
}

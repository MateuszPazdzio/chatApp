using chat.Server.models;

namespace chat.Server.services
{
    public interface IUserService
    {
        Task<LoginResponse> LoginUser(User user);
        Task<LoginResponse> LoginApi(User user);
        //public Task<bool> GetUserByEmail(string email);
        //public Task<bool> GetUserByUserName(string value);
        public Task<RegisterResponse> CreateUser(User user);
        public Task LogoutAsync();
        Task<User> GetUser(string searchPhrase);
        //public Task<Status> LoginApi(ApplicationUser user);
    }
}
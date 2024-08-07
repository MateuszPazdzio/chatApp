using chat.Server.models;

namespace chat.Server.services
{
    public interface IUserService
    {
        Task<Status> LoginUser(User user);
        Task<Status> LoginApi(User user);
        //public Task<bool> GetUserByEmail(string email);
        //public Task<bool> GetUserByUserName(string value);
        public Task<Status> CreateUser(User user);
        public Task LogoutAsync();
        //public Task<Status> LoginApi(ApplicationUser user);
    }
}
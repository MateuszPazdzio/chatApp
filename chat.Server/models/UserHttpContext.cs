using System.Security.Claims;

namespace chat.Server.models
{
    public class UserHttpContext : IUserHttpContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHttpContext(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public CurrentUser GetUser()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if(user == null)
            {
                throw new Exception("User does not exists");
            }

            CurrentUser currentUser;

            try
            {
                currentUser = new CurrentUser()
                {
                    UserName = user.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                    Id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value,
                    ////Email = user.FindFirst(c => c.Type == ClaimTypes.Email).Value,
                };
            }
            catch (Exception e)
            {
                return null;
            }

            return currentUser;
        }
    }
}

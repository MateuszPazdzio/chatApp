using chat.Server.Data;
using chat.Server.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace chat.Server.services
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ChatDbContext _context;
        Dictionary<string, Func<User, string>> searchByDict = new Dictionary<string, Func<User, string>>()
            {
                {"userName",user=>user.UserName},
                {"email",user=>user.Email},
            };
        public UserService(ChatDbContext context, IPasswordHasher<User> passwordHasher,
            UserManager<User> userManager, SignInManager<User> signInManager, AuthenticationSettings authenticationSettings,
            IUserHttpContext userHttpContext) : base(context) 
        {
            _context = context;
            this._passwordHasher = passwordHasher;
            this._authenticationSettings = authenticationSettings;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public async Task<RegisterResponse> CreateUser(User user)
        {
            var response = new RegisterResponse();

            var userName = await _userManager.FindByNameAsync(user.UserName);
            if (userName != null)
            {
                response.IsRegistered = false;
                response.ResponseMessage = $"Username {user.UserName} already exist";
                return response;
            }

            var userEmail = await _context.Users.FirstOrDefaultAsync(u=>u.Email==user.Email);
            if (userEmail != null)
            {
                response.ResponseMessage = $"Email {user.Email} already exist";
                response.IsRegistered = false;
                return response;
            }

            var result = await _userManager.CreateAsync(user, user.Password);
            if (!result.Succeeded)
            {
                response.IsRegistered = false;
                response.ResponseMessage = String.Join("\n",result.Errors.ToList().Select(e=>e.Description));
                return response;
            }

            //await _userManager.AddToRoleAsync(user, "Member");
            response.IsRegistered = true;
            response.ResponseMessage = "You have registered successfully";
            return response;
        }

        public async Task<LoginResponse> LoginUser(User user)

        {
            var response = new LoginResponse();
            var userFound = await _userManager.FindByNameAsync(user.UserName);

            if (user == null)
            {
                response.IsLoggedIn = false;
                response.ResponseMessage = "Invalid username";
                return response;
            }

            if (!await _userManager.CheckPasswordAsync(userFound, user.Password))
            //if (!await _userManager.CheckPasswordAsync(userFound, user.Password))
            {
                response.IsLoggedIn = false;
                response.ResponseMessage = "Invalid Password";
                return response;
            }
            //var signInResult = await _signInManager.PasswordSignInAsync(user, userFound.Password, false, true);
            var signInResult = await _signInManager.PasswordSignInAsync(userFound, user.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                response.IsLoggedIn = true;
                response.ResponseMessage = "Logged in successfully";
            }
            else if (signInResult.IsLockedOut)
            {
                response.IsLoggedIn = false;
                response.ResponseMessage = "User is locked out";
            }
            else
            {
                response.IsLoggedIn = false;
                response.ResponseMessage= "Error on logging in";
            }

            return response;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<LoginResponse> LoginApi(User user)
        {
            var validUser = await AuthenticateUser(user);
            if(validUser == null)
            {
                return new LoginResponse()
                {
                    IsLoggedIn = false,
                    Token=null,
                    ResponseMessage= "Wrong password or login"
                };
            }
            var claims = await GenerateListOfClaims(validUser);
            var token = await GenerateToken(claims);

            return new LoginResponse()
            {
                IsLoggedIn = true,
                Token = token.ToString(),
                ResponseMessage = "Succesfully logged in"
            };
        }
        private async Task<User> AuthenticateUser(User userMapped)
        {
            var validUser = await _context.Users
                .FirstOrDefaultAsync(user => user.UserName == userMapped.UserName);
            if (validUser == null)
            {
                //throw new WrongCredentialsException("Email does not exists");
                return null;
            }
            try
            {
                var result = _passwordHasher.VerifyHashedPassword(validUser, validUser.PasswordHash, userMapped.Password);

            }
            catch (Exception)
            {

                throw;
            }
            //if (PasswordVerificationResult.Failed == result)
            //{
            //    //throw new Exception("Wrong password");
            //    return null;
            //}
            return validUser;
        }
        private async Task<List<Claim>> GenerateListOfClaims(User validUser)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,validUser.Id.ToString()),
                new Claim(ClaimTypes.Email,validUser.Email),
                new Claim(ClaimTypes.Name, validUser.UserName),
                new Claim(ClaimTypes.Role, String.Join(',',await _userManager.GetRolesAsync(validUser))),
            };
        }
        private async Task<string> GenerateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer,
                claims: claims, expires: expires, signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetUser(string searchPhrase)
        {
            return await this._context.Users.FirstOrDefaultAsync(user => searchByDict[searchPhrase].Invoke(user) != null);
        }
    }
}

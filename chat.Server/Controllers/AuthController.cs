using AutoMapper;
using chat.Server.models;
using chat.Server.models.dtos;
using chat.Server.services;
using Microsoft.AspNetCore.Mvc;

namespace chat.Server.Controllers
{
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userRepository;
        private readonly IMapper _mapper;


        public AuthController(IUserService userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }
        [HttpPost]
        [Route("auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDtoApi loginUserDtoApi)
        {
            var loginUser = _mapper.Map<User>(loginUserDtoApi);
            var status = await this._userRepository.LoginApi(loginUser);
            return Ok(status);
        }
        [HttpPost]
        [Route("auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDtoApi registerUserDto)
        {
            var registerUser= _mapper.Map<User>(registerUserDto);
            var status =await _userRepository.CreateUser(registerUser);
            return Ok(status);
        }

        [HttpPost]
        [Route("auth/logout")]
        public async Task<IActionResult> Logut([FromBody] RegisterUserDtoApi registerUserDto)
        {
            await _userRepository.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}

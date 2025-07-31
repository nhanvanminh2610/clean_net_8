using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request) => await _userService.RegisterAsync(request);

        [HttpPost("log-in")]
        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest) => await _userService.LoginAsync(loginRequest);
    }
}

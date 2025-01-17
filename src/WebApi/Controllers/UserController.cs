using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            return await _userService.RegisterAsync(request);
        }
    }
}

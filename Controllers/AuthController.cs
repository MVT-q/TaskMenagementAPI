using Microsoft.AspNetCore.Mvc;
using TaskMenagementAPI.DTOs.Auth;
using TaskMenagementAPI.Services;

namespace TaskMenagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto?>> Login(LoginDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
                return Unauthorized();

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            await _authService.RegisterAsync(request);

            return Ok();
        }
    }
}

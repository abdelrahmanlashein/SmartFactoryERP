using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Identity.Models;
using SmartFactoryERP.Application.Interfaces.Identity;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            return Ok(await _authService.Register(request));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            return Ok(await _authService.Login(request));
        }
    }
}


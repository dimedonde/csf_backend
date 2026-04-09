using Microsoft.AspNetCore.Mvc;
using ApiGateway.DTOs;
using ApiGateway.Services;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwt;
        private readonly IConfiguration _config;

        public AuthController(JwtService jwt, IConfiguration config)
        {
            _jwt = jwt;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest(new { message = "Username y Password son requeridos" });

            var username = _config["Auth:Username"];
            var password = _config["Auth:Password"];

            if (user.Username == username && user.Password == password)
            {
                var token = _jwt.GenerateToken(user.Username);
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Fiap.Api.ControleCarbono.Services;
using Fiap.Api.ControleCarbono.Models;
using Fiap.Api.ControleCarbono.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Fiap.Api.ControleCarbono.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; 
        private readonly IUsuarioService _usuarioService;

        public AuthController(IAuthService authService, IUsuarioService usuarioService)
        {
            _authService = authService;
            _usuarioService = usuarioService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            var usuario = await _usuarioService.Authenticate(login.Username, login.Password);
            
            if (usuario == null)
                return Unauthorized(new { message = "Usuário ou senha incorretos" });

            var token = _authService.GenerateToken(usuario);
            
            return Ok(new { 
                usuario.Id,
                usuario.Username,
                usuario.Nome,
                usuario.Email,
                usuario.Role,
                Token = token 
            });
        }
    }
}

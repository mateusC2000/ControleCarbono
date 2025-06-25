using Microsoft.AspNetCore.Mvc;
using Fiap.Api.ControleCarbono.Services;
using Fiap.Api.ControleCarbono.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Fiap.Api.ControleCarbono.Models;

namespace Fiap.Api.ControleCarbono.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioViewModel viewModel)
        {
            var usuario = _mapper.Map<Usuario>(viewModel);
            
            try
            {
                var createdUser = await _usuarioService.Create(usuario, viewModel.Password);
                return Ok(new { createdUser.Id, createdUser.Username });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuarioService.GetById(id);
            if (usuario == null) return NotFound();
            
            return Ok(usuario);
        }
    }
}
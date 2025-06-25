using Fiap.Api.ControleCarbono.Models;
using Fiap.Api.ControleCarbono.Services;
using Fiap.Api.ControleCarbono.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Fiap.Api.ControleCarbono.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmissaoCarbonoController : ControllerBase
    {
        private readonly IEmissaoCarbonoService _service;
        private readonly IMapper _mapper;

        public EmissaoCarbonoController(IEmissaoCarbonoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmissaoCarbonoViewModel>>> Get()
        {
            var emissoes = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<EmissaoCarbonoViewModel>>(emissoes));
        }

        [HttpGet("empresa/{empresaId}")]
        public async Task<ActionResult<IEnumerable<EmissaoCarbonoViewModel>>> GetByEmpresa(int empresaId)
        {
            var emissoes = await _service.GetByEmpresaIdAsync(empresaId);
            return Ok(_mapper.Map<IEnumerable<EmissaoCarbonoViewModel>>(emissoes));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmissaoCarbonoViewModel>> Get(int id)
        {
            var emissao = await _service.GetByIdAsync(id);
            if (emissao == null)
                return NotFound();

            return Ok(_mapper.Map<EmissaoCarbonoViewModel>(emissao));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmissaoCarbonoViewModel viewModel)
        {
            var emissao = _mapper.Map<EmissaoCarbono>(viewModel);
            await _service.AddAsync(emissao);

            var createdViewModel = _mapper.Map<EmissaoCarbonoViewModel>(emissao);
            return CreatedAtAction(nameof(Get), new { id = emissao.Id }, createdViewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EmissaoCarbonoViewModel viewModel)
        {
            var emissao = _mapper.Map<EmissaoCarbono>(viewModel);
            emissao.Id = id;

            var success = await _service.UpdateAsync(id, emissao);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.RemoveAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}

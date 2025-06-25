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
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _service;
        private readonly IMapper _mapper;

        public EmpresaController(IEmpresaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpresaViewModel>>> Get()
        {
            var empresas = await _service.GetAllAsync();
            var resultado = _mapper.Map<IEnumerable<EmpresaViewModel>>(empresas);
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpresaViewModel>> Get(int id)
        {
            var empresa = await _service.GetByIdAsync(id);
            if (empresa == null)
                return NotFound();

            return Ok(_mapper.Map<EmpresaViewModel>(empresa));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmpresaViewModel viewModel)
        {
            var empresa = _mapper.Map<Empresa>(viewModel);
            await _service.AddAsync(empresa);

            var createdViewModel = _mapper.Map<EmpresaViewModel>(empresa);
            return CreatedAtAction(nameof(Get), new { id = empresa.Id }, createdViewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EmpresaViewModel viewModel)
        {
            var empresa = _mapper.Map<Empresa>(viewModel);
            empresa.Id = id;

            var success = await _service.UpdateAsync(id, empresa);
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

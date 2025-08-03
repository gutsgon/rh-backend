
using Microsoft.AspNetCore.Mvc;
using Rh_Backend.DTO;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _funcionarioService;

        public FuncionarioController(IFuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosFuncionarios()
        {
            var funcionarios = await _funcionarioService.ListarTodosFuncionarios();
            if (funcionarios == null) return NotFound();
            return Ok(funcionarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterFuncionarioPorId([FromQuery] long id)
        {
            var funcionario = await _funcionarioService.BuscarFuncionarioPorId(id);
            if (funcionario == null) return NotFound();
            return Ok(funcionario);
        }

        [HttpPost("/filtrar")]
        public async Task<IActionResult> FiltrarFuncionarios([FromBody] FuncionarioCreateDTO funcionario)
        {
            var funcionarios = await _funcionarioService.BuscarFuncionarios(funcionario);
            if (funcionarios == null) return NotFound();
            return Ok(funcionarios);
        }

        [HttpGet("/detalhes")]
        public async Task<IActionResult> ObterFuncionarioDetalhado(long id)
        {
            var funcionarioDetalhes = await _funcionarioService.BuscarFuncionarioDetalhado(id);
            if (funcionarioDetalhes == null) return NotFound();
            return Ok(funcionarioDetalhes);
        }

        [HttpGet("/cargo/{cargo}")]
        public async Task<IActionResult> ObterFuncionariosPorCargo(string cargo)
        {
            var funcionarios = await _funcionarioService.BuscarFuncionariosPorCargo(cargo);
            if (funcionarios == null) return NotFound();
            return Ok(funcionarios);
        }

        [HttpGet("/cargo")]
        public async Task<IActionResult> ObterFuncionariosComCargo()
        {
            var funcionarios = await _funcionarioService.BuscarFuncionariosComCargo();
            if (funcionarios == null) return NotFound();
            return Ok(funcionarios);
        }

        [HttpGet("/salario")]
        public async Task<IActionResult> CalcularMediaSalario()
        {
            var mediaSalario = await _funcionarioService.CalcularMediaSalario();
            if (mediaSalario == null) return NotFound();
            return Ok(mediaSalario);
        }

        [HttpPost]
        public async Task<IActionResult> CriarFuncionario([FromBody] FuncionarioComCargoCreateDTO funcionario)
        {
            if (funcionario == null) return BadRequest("Dados inválidos.");
            var novoFuncionario = await _funcionarioService.CriarFuncionario(funcionario);
            return CreatedAtAction(nameof(ObterFuncionarioPorId), new { id = novoFuncionario.Id }, novoFuncionario);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarFuncionario([FromBody] FuncionarioComCargoUpdateDTO funcionario)
        {
            if (funcionario == null) return BadRequest("Dados inválidos.");
            var funcionarioAtualizado = await _funcionarioService.AtualizarFuncionario(funcionario);
            if (funcionarioAtualizado == null) return NotFound();
            return Ok(funcionarioAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarFuncionario(long id)
        {
            var resultado = await _funcionarioService.DeletarFuncionario(id);
            if (!resultado) return NotFound();
            return NoContent();
        }
    }
}
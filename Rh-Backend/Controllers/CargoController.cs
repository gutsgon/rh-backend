
using Microsoft.AspNetCore.Mvc;
using Rh_Backend.DTO;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargoController : ControllerBase
    {
        private readonly ICargoService _cargoService;

        public CargoController(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosCargos()
        {
            var cargos = await _cargoService.ListarTodosCargos();
            if (cargos == null) return NotFound();
            return Ok(cargos);
        }

        [HttpGet("/cargo/nome/{nome}")]
        public async Task<IActionResult> ObterCargoPorNome(string nome)
        {
            var cargo = await _cargoService.BuscarCargoPorNome(nome);
            if (cargo == null) return NotFound();
            return Ok(cargo);
        }

        [HttpGet("/cargo/{id}")]
        public async Task<IActionResult> ObterCargoPorId(long id)
        {
            var cargo = await _cargoService.BuscarCargoPorId(id);
            if (cargo == null) return NotFound();
            return Ok(cargo);
        }

        [HttpPost]
        public async Task<IActionResult> CriarCargo([FromBody] CargoCreateDTO cargo)
        {
            if (cargo == null) return BadRequest("Dados inválidos.");
            var novoCargo = await _cargoService.CriarCargo(cargo);
            return CreatedAtAction(nameof(ObterCargoPorId), new { id = novoCargo.Id }, novoCargo);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarCargo([FromBody] CargoUpdateDTO cargo)
        {
            if (cargo == null) return BadRequest("Dados inválidos.");
            var novoCargo = await _cargoService.AtualizarCargo(cargo);
            if (novoCargo == null) return NotFound();
            return Ok(novoCargo);
        }

        [HttpDelete("/cargo/{id}")]
        public async Task<IActionResult> DeletarCargo(long id)
        {
            if (!await _cargoService.Exists(id)) return NotFound();
            var resultado = await _cargoService.DeletarCargo(id);
            if (!resultado) return BadRequest("Erro ao deletar o cargo.");
            return NoContent();
        }
    }
}
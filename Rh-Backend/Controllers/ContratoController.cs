
using Microsoft.AspNetCore.Mvc;
using Rh_Backend.DTO;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratoController : ControllerBase
    {
        private readonly IContratoService _contratoService;
        public ContratoController(IContratoService contratoService)
        {
            _contratoService = contratoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosContratos()
        {
            var contratos = await _contratoService.ListarTodosContratos();
            if (contratos == null) return NotFound();
            return Ok(contratos);
        }

        [HttpGet("/{idCargo}/{idFuncionario}")]
        public async Task<IActionResult> ObterContratoById(long idCargo, long idFuncionario)
        {
            var contrato = await _contratoService.BuscarContratoPorId(idCargo, idFuncionario);
            if (contrato == null) return NotFound();
            return Ok(contrato);
        }

        [HttpPost]
        public async Task<IActionResult> CriarContrato([FromBody] ContratoDTO contrato)
        {
            if (contrato == null) return BadRequest("Dados inválidos!");
            var novoContrato = await _contratoService.CriarContrato(contrato);
            if (novoContrato == null) return NotFound();
            return CreatedAtAction(nameof(ObterContratoById), new { idCargo = novoContrato.IdCargo, idFuncionario = novoContrato.IdFuncionario }, novoContrato);
        }

        [HttpPut("/{idFuncionario}/{idCargoAtigo}/{idCargoNovo}")]
        public async Task<IActionResult> AtualizarContrato(long idFuncionario, long idCargoAntigo, long idCargoNovo)
        {
            var contrato = await _contratoService.AtualizarContrato(idFuncionario, idCargoAntigo, idCargoNovo);
            if (contrato == null) return NotFound();
            return Ok(contrato);
        }

        [HttpDelete("/{idCargo}/{idFuncionario}")]
        public async Task<IActionResult> DeletarContrato(long idCargo, long idFuncionario)
        {
            if (!await _contratoService.ExistsPorId(idCargo,idFuncionario)) return NotFound();
            var resultado = await _contratoService.DeletarContrato(idCargo, idFuncionario);
            if (!resultado) return BadRequest("Erro ao deletar o cargo.");
            return NoContent();
        }


    }
}
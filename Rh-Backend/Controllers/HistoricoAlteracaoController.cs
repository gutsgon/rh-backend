
using Microsoft.AspNetCore.Mvc;
using Rh_Backend.DTO;
using Rh_Backend.Services;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class HistoricoAlteracaoController : ControllerBase
    {
        private readonly IHistoricoAlteracaoService _historicoAlteracaoService;
        public HistoricoAlteracaoController(IHistoricoAlteracaoService historicoAlteracaoService)
        {
            _historicoAlteracaoService = historicoAlteracaoService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodosHistoricos()
        {
            var historico = await _historicoAlteracaoService.ListarTodosHistoricos();
            if (historico == null) return NotFound();
            return Ok(historico);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> ObterHistoricoPorId(long id)
        {
            var historico = await _historicoAlteracaoService.BuscarHistoricoPorId(id);
            if (historico == null) return NotFound();
            return Ok(historico);
        }

        [HttpGet("/{idFuncionario}/{idCargo}/{idFerias}")]
        public async Task<IActionResult> ObterHistoricos(long idFuncionario, long? idCargo, long? idFerias)
        {
            var historico = await _historicoAlteracaoService.BuscarHistoricos(idFuncionario, idCargo, idFerias);
            if (historico == null) return NotFound();
            return Ok(historico);
        }

        [HttpPost("/data")]
        public async Task<IActionResult> ObterPorData([FromBody] HistoricoAlteracaoCreateDTO historico)
        {
            if (historico == null) return BadRequest("Dados inválidos!");
            var historicoDTO = await _historicoAlteracaoService.BuscarHistoricoPorData(historico.DataAlteracao);
            if (historicoDTO == null) return NotFound();
            return Ok(historicoDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CriarHistoricoALteracao([FromBody] HistoricoAlteracaoCreateDTO historico)
        {
            if (historico == null) return BadRequest("Dados inválidos!");
            var historicoDTO = await _historicoAlteracaoService.CriarHistorico(historico);
            if (historicoDTO == null) return NotFound();
            return CreatedAtAction(nameof(ObterHistoricoPorId), new { id = historicoDTO.Id }, historicoDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarHistoricoAlteracao(long id)
        {
            if (!await _historicoAlteracaoService.Exists(id)) return NotFound();
            var resultado = await _historicoAlteracaoService.DeletarHistorico(id);
            if (!resultado) return BadRequest("Erro ao deletar historico");
            return NoContent();
        }


    }
}
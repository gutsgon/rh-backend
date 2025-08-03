
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Rh_Backend.DTO;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeriasController : ControllerBase
    {
        private readonly IFeriasService _feriasService;
        public FeriasController(IFeriasService feriasService)
        {
            _feriasService = feriasService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasFerias()
        {
            var ferias = await _feriasService.ListarTodasFerias();
            return Ok(ferias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterFeriasPorId(long id)
        {
            var ferias = await _feriasService.BuscarFeriasPorId(id);
            return Ok(ferias);
        }

        [HttpGet("/idFuncionario/{idFuncionario}")]
        public async Task<IActionResult> ObterFeriasPorIdFuncionario(long idFuncionario)
        {
            var ferias = await _feriasService.BuscarFeriasPorIdFuncionario(idFuncionario);
            return Ok(ferias);
        }

        [HttpPost("/obter-data")]
        public async Task<IActionResult> ObterFeriasPorData([FromBody] FeriasCreateDTO ferias)
        {
            var feriasDTO = await _feriasService.BuscarFeriasPorData(ferias.DataInicio, ferias.DataFim);
            return Ok(feriasDTO);
        }

        [HttpGet("/status/{status}")]
        public async Task<IActionResult> ObterFeriasPorStatus(string status)
        {
            var ferias = await _feriasService.BuscarFeriasPorStatus(status);
            return Ok(ferias);
        }

        [HttpGet("idFuncionario/status/{idFuncionario}/{status}")]
        public async Task<IActionResult> ObterFeriasPorIdFuncionarioEStatus(long idFuncionario, string status)
        {
            var ferias = await _feriasService.BuscarFeriasPorFuncionarioEStatus(idFuncionario, status);
            return Ok(ferias);
        }

        [HttpPost]
        public async Task<IActionResult> CriarFerias([FromBody] FeriasCreateDTO ferias)
        {
            if (ferias == null) return BadRequest("Dados inválidos!");
            var feriasDTO = await _feriasService.CriarFerias(ferias);
            return CreatedAtAction(nameof(ObterFeriasPorId), new { id = feriasDTO.Id }, feriasDTO);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarFerias([FromBody] FeriasUpdateDTO ferias)
        {
            if (ferias == null) return BadRequest("Dados inválidos!");
            var feriasDTO = await _feriasService.AtualizarFerias(ferias);
            if (feriasDTO == null) return NotFound();
            return Ok(feriasDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarFerias(long id)
        {
            if (!await _feriasService.Exists(id)) return NotFound();
            var resultado = await _feriasService.DeletarFerias(id);
            if (!resultado) return BadRequest("Erro ao deletar ferias");
            return NoContent();
        }
    }
}
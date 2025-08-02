using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface IContratoService
    {
        public Task<IEnumerable<ContratoDTO>> ListarTodosContratos();
        public Task<ContratoDTO?> BuscarContratoPorId(long id);
        public Task<ContratoDTO> CriarContrato(ContratoDTO contrato);
        public Task<ContratoDTO> AtualizarContrato(ContratoDTO contrato);
        public Task<bool> DeletarContrato(long id);
    }
}
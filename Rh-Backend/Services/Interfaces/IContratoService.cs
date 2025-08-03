using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface IContratoService
    {
        Task<IEnumerable<ContratoDTO>> ListarTodosContratos();
        Task<ContratoDTO?> BuscarContratoPorId(long idCargo, long idFuncionario);
        Task<ContratoDTO> CriarContrato(ContratoDTO contrato);
        Task<ContratoDTO> AtualizarContrato(long idFuncionario, long idCargoAntigo, long idCargoNovo);
        Task<bool> DeletarContrato(long idCargo, long idFuncionario);
        Task<bool> ExistsPorId(long idCargo, long idFuncionario);
    }
}
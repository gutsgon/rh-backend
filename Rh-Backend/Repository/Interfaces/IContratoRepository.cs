using Rh_Backend.Models;

namespace Rh_Backend.Repository.Interfaces
{
    public interface IContratoRepository
    {
        Task<IEnumerable<ContratoModel>> GetAllAsync();
        Task<ContratoModel?> GetByIdAsync(long idCargo, long idFuncionario);
        Task<ContratoModel> CreateAsync(ContratoModel contrato);
        Task<ContratoModel> UpdateAsync(long idFuncionario, long idCargoAntigo, long idCargoNovo);
        Task<bool> DeleteAsync(long idCargo, long idFuncionario);
        Task<bool> ExistsAsync(long idCargo, long idFuncionario);
        Task<bool> ExistsCargoAsync(long idCargo);
        Task<bool> ExistsFuncionarioAsync(long idFuncionario);

    }
}
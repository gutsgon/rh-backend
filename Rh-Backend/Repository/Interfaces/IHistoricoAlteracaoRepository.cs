using Rh_Backend.Models;

namespace Rh_Backend.Repository.Interfaces
{
    public interface IHistoricoAlteracaoRepository
    {
        Task<IEnumerable<HistoricoAlteracaoModel>> GetAllAsync();
        Task<HistoricoAlteracaoModel?> GetByIdAsync(long id);
        Task<IEnumerable<HistoricoAlteracaoModel?>> SearchByIdAsync(long idFuncionario, long? idCargo, long? idFerias);
        Task<IEnumerable<HistoricoAlteracaoModel?>> GetByDataAlteracaoAsync(DateTime dataAlteracao);
        Task<HistoricoAlteracaoModel> CreateAsync(HistoricoAlteracaoModel historicoAlteracao);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
    }
}
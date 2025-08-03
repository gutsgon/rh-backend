using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface IHistoricoAlteracaoService
    {
        public Task<IEnumerable<HistoricoAlteracaoCreateDTO>> ListarTodosHistoricos();
        public Task<HistoricoAlteracaoReadDTO?> BuscarHistoricoPorId(long id);
        public Task<IEnumerable<HistoricoAlteracaoReadDTO>> BuscarHistoricos(long idFuncionario, long? idCargo, long? idFerias);
        public Task<IEnumerable<HistoricoAlteracaoReadDTO>> BuscarHistoricoPorData(DateTime dataAlteracao);
        public Task<HistoricoAlteracaoReadDTO> CriarHistorico(HistoricoAlteracaoCreateDTO historicoAlteracao);
        public Task<bool> DeletarHistorico(long id);
        public Task<bool> Exists(long id);
    }
}
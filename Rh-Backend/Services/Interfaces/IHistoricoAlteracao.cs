using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface IHistoricoAlteracao
    {
        public Task<IEnumerable<HistoricoAlteracaoCreateDTO>> ListarTodosHistoricos();
        public Task<HistoricoAlteracaoReadDTO?> BuscarHistoricoPorId(long id);
        public Task<IEnumerable<HistoricoAlteracaoReadDTO>> BuscarHistoricos(long idFuncionario, long? idCargo, long? idFerias);
        public Task<IEnumerable<HistoricoAlteracaoReadDTO>> BuscarHistoricoPorData(DateTime dataAlteracao);
        public Task<HistoricoAlteracaoReadDTO> CriarHistorico(HistoricoAlteracaoCreateDTO historicoAlteracao);
        public Task<HistoricoAlteracaoReadDTO> AtualizarHistorico(HistoricoAlteracaoUpdateDTO historicoAlteracao);
        public Task<bool> DeletarHistorico(long id);
    }
}
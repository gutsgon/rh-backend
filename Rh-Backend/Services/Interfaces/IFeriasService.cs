using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface IFeriasService
    {
        public Task<IEnumerable<FeriasReadDTO>> ListarTodasFerias();
        public Task<FeriasReadDTO?> BuscarFeriasPorId(long id);
        public Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorIdFuncionario(long idFuncionario);
        public Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorData(DateTime dataInicio, DateTime dataFim);
        public Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorStatus(string status);
        public Task<FeriasReadDTO> CriarFerias(FeriasCreateDTO ferias);
        public Task<FeriasReadDTO> AtualizarFerias(FeriasUpdateDTO ferias);
        public Task<bool> DeletarFerias(long id);
    }
}
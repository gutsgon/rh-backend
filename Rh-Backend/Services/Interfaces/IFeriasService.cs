using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface IFeriasService
    {
        Task<IEnumerable<FeriasReadDTO>> ListarTodasFerias();
        Task<FeriasReadDTO?> BuscarFeriasPorId(long id);
        Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorIdFuncionario(long idFuncionario);
        Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorData(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorStatus(string status);
        Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorFuncionarioEStatus(long idFuncionario, string status);
        Task<FeriasReadDTO> CriarFerias(FeriasCreateDTO ferias);
        Task<FeriasReadDTO> AtualizarFerias(FeriasUpdateDTO ferias);
        Task<bool> DeletarFerias(long id);
        Task<bool> Exists(long id);
    }
}
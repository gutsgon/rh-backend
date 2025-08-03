using Rh_Backend.Models;

namespace Rh_Backend.Repository.Interfaces
{
    public interface IFeriasRepository
    {
        Task<IEnumerable<FeriasModel>> GetAllAsync();
        Task<FeriasModel?> GetByIdAsync(long id);
        Task<IEnumerable<FeriasModel>> GetByIdFuncionarioAsync(long idFuncionario);
        Task<IEnumerable<FeriasModel>> GetByDataAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<FeriasModel>> GetByStatusAsync(string status);
        Task<IEnumerable<FeriasModel>> GetByFuncionarioAndStatusAsync(long idFuncionario, string status);
        Task<FeriasModel> CreateAsync(FeriasModel ferias);
        Task<FeriasModel> UpdateAsync(FeriasModel ferias);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
    }
}
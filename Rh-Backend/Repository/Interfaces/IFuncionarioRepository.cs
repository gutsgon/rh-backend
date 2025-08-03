
using Rh_Backend.DTO;
using Rh_Backend.Models;

namespace Rh_Backend.Repository.Interfaces
{
    public interface IFuncionarioRepository
    {
        Task<IEnumerable<FuncionarioModel>> GetAllAsync();
        Task<FuncionarioDetalhesDTO> GetWithDetailsAsync(long id);
        Task<IEnumerable<FuncionarioComCargoDTO>> GetFuncionariosWithCargo();
        Task<IEnumerable<FuncionarioModel>> SearchAsync(DateTime? dataAdmissao, string nome, decimal salario, bool status);
        Task<IEnumerable<FuncionarioModel>> GetByCargoAsync(string cargo);
        Task<FuncionarioModel?> GetByIdAsync(long id);
        Task<FuncionarioModel> CreateAsync(FuncionarioModel funcionario);
        Task<FuncionarioModel> UpdateAsync(FuncionarioModel funcionario);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
    }
}
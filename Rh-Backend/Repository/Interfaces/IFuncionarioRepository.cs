
using Rh_Backend.DTO;
using Rh_Backend.Models;

namespace Rh_Backend.Repository.Interfaces
{
    public interface IFuncionarioRepository
    {
        public Task<IEnumerable<FuncionarioModel>> GetAllAsync();
        public Task<FuncionarioDetalhesDTO> GetWithDetailsAsync(long id);
        public Task<IEnumerable<FuncionarioModel>> SearchAsync(DateTime? dataAdmissao, string nome, decimal salario, bool status);
        public Task<IEnumerable<FuncionarioModel>> GetByCargoAsync(string cargo);
        public Task<FuncionarioModel?> GetByIdAsync(long id);
        public Task<FuncionarioModel> CreateAsync(FuncionarioModel funcionario);
        public Task<FuncionarioModel> UpdateAsync(FuncionarioModel funcionario);
        public Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
    }
}
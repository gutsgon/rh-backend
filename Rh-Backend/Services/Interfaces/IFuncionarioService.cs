using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface IFuncionarioService
    {
        public Task<IEnumerable<FuncionarioReadDTO>> ListarTodosFuncionarios();
        public Task<IEnumerable<FuncionarioReadDTO>> BuscarFuncionarios(DateTime? dataAdmissao, string nome, decimal salario, bool status);
        public Task<FuncionarioDetalhesDTO> BuscarFuncionarioDetalhado(long id);
        public Task<IEnumerable<FuncionarioReadDTO>> BuscarFuncionariosPorCargo(string cargo);
        public Task<FuncionarioReadDTO?> BuscarFuncionarioPorId(long id);
        public Task<FuncionarioReadDTO> CriarFuncionario(FuncionarioCreateDTO funcionario, CargoCreateDTO cargo);
        public Task<FuncionarioReadDTO> AtualizarFuncionario(FuncionarioUpdateDTO funcionario);
        public Task<bool> DeletarFuncionario(long id);
    }
}
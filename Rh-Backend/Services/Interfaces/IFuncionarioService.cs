using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface IFuncionarioService
    {
        public Task<IEnumerable<FuncionarioReadDTO>> ListarTodosFuncionarios();
        public Task<IEnumerable<FuncionarioReadDTO>> BuscarFuncionarios(FuncionarioCreateDTO funcionario);
        public Task<FuncionarioDetalhesDTO> BuscarFuncionarioDetalhado(long id);
        public Task<IEnumerable<FuncionarioReadDTO>> BuscarFuncionariosPorCargo(string cargo);
        public Task<IEnumerable<FuncionarioComCargoDTO>> BuscarFuncionariosComCargo();
        public Task<FuncionarioReadDTO?> BuscarFuncionarioPorId(long id);
        public Task<FuncionarioMediaSalarioDTO> CalcularMediaSalario();
        public Task<FuncionarioComCargoReadDTO> CriarFuncionario(FuncionarioComCargoCreateDTO funcionario);
        public Task<FuncionarioComCargoReadDTO> AtualizarFuncionario(FuncionarioComCargoUpdateDTO funcionario);
        public Task<bool> DeletarFuncionario(long id);
        Task<bool> Exists(long id);
    }
}
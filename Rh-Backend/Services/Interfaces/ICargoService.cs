using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface ICargoService
    {
        public Task<IEnumerable<CargoReadDTO>> ListarTodosCargos();
        public Task<CargoReadDTO?> BuscarCargoPorNome(string nome);
        public Task<CargoReadDTO?> BuscarCargoPorId(long id);
        public Task<CargoReadDTO> CriarCargo(CargoCreateDTO cargo);
        public Task<CargoReadDTO> AtualizarCargo(CargoUpdateDTO cargo);
        public Task<bool> DeletarCargo(long id);
    }
}
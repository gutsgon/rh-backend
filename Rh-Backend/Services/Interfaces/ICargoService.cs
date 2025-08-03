using Rh_Backend.DTO;

namespace Rh_Backend.Services.Interfaces
{
    public interface ICargoService
    {
        Task<IEnumerable<CargoReadDTO>> ListarTodosCargos();
        Task<CargoReadDTO?> BuscarCargoPorNome(string nome);
        Task<CargoReadDTO?> BuscarCargoPorId(long id);
        Task<CargoReadDTO> CriarCargo(CargoCreateDTO cargo);
        Task<CargoReadDTO> AtualizarCargo(CargoUpdateDTO cargo);
        Task<bool> ExistsPorNome(string nome);
        Task<bool> Exists(long id);
        Task<bool> DeletarCargo(long id);
    }
}
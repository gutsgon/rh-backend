using Rh_Backend.Models;

namespace Rh_Backend.Repository.Interfaces
{
    public interface ICargoRepository
    {
        Task<IEnumerable<CargoModel>> GetAllAsync();
        Task<CargoModel?> GetByNomeAsync(string nome);
        Task<CargoModel?> GetByIdAsync(long id);
        Task<CargoModel> CreateAsync(CargoModel cargo);
        Task<CargoModel> UpdateAsync(CargoModel cargo);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
        Task<bool> ExistsByNomeAsync(string nome);

    }
}
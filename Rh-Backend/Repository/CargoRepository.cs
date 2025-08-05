using Microsoft.EntityFrameworkCore;
using Rh_Backend.Data;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;

namespace Rh_Backend.Repository 
{
    public class CargoRepository : ICargoRepository
    {
        private readonly AppDbContext _context;
        public CargoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CargoModel>> GetAllAsync()
        {
            return await _context.Cargo.ToListAsync();
        }

        public async Task<CargoModel?> GetByNomeAsync(string nome)
        {
            return await _context.Cargo.FirstOrDefaultAsync(c => c.Nome == nome);
        }

        public async Task<CargoModel?> GetByIdAsync(long id)
        {
            return await _context.Cargo.FindAsync(id);
        }

        public async Task<CargoModel> CreateAsync(CargoModel cargo)
        {
            _context.Cargo.Add(cargo);
            await _context.SaveChangesAsync();
            return cargo;
        }

        public async Task<CargoModel> UpdateAsync(CargoModel cargo)
        {
            _context.Cargo.Update(cargo);
            await _context.SaveChangesAsync();
            return cargo;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var cargo = await GetByIdAsync(id);
            if (cargo == null)
            {
                return false;
            }

            _context.Cargo.Remove(cargo);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
            return true;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Cargo.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsByNomeAsync(string nome)
        {
            return await _context.Cargo.AsNoTracking().AnyAsync(c => c.Nome == nome);
        }
    }
}
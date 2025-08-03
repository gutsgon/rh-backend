using Microsoft.EntityFrameworkCore;
using Rh_Backend.Data;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;

namespace Rh_Backend.Repository
{
    public class FeriasRepository : IFeriasRepository
    {
        private readonly AppDbContext _context;
        public FeriasRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FeriasModel>> GetAllAsync()
        {
            return await _context.Ferias.ToListAsync();
        }

        public async Task<FeriasModel?> GetByIdAsync(long id)
        {
            return await _context.Ferias.FindAsync(id);
        }

        public async Task<IEnumerable<FeriasModel>> GetByIdFuncionarioAsync(long idFuncionario)
        {
            return await _context.Ferias
                .Where(f => f.IdFuncionario == idFuncionario)
                .ToListAsync();
        }

        public async Task<IEnumerable<FeriasModel>> GetByDataAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.Ferias
                .Where(f => f.DataInicio >= dataInicio && f.DataFim <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<FeriasModel>> GetByStatusAsync(string status)
        {
            return await _context.Ferias
                .Where(f => f.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<FeriasModel>> GetByFuncionarioAndStatusAsync(long idFuncionario, string status)
        {
            return await _context.Ferias
                .Where(f => f.IdFuncionario == idFuncionario && f.Status == status)
                .ToListAsync();
        }

        public async Task<FeriasModel> CreateAsync(FeriasModel ferias)
        {
            _context.Ferias.Add(ferias);
            await _context.SaveChangesAsync();
            return ferias;
        }

        public async Task<FeriasModel> UpdateAsync(FeriasModel ferias)
        {
            _context.Entry(ferias).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return ferias;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var ferias = await GetByIdAsync(id);
            if (ferias == null)
            {
                return false;
            }

            _context.Ferias.Remove(ferias);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Ferias.AnyAsync(c => c.Id == id);
        }
        
    }
}
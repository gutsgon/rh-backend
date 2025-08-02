
using Microsoft.EntityFrameworkCore;
using Rh_Backend.Data;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;

namespace Rh_Backend.Repository
{
    public class ContratoRepository : IContratoRepository
    {
        private readonly AppDbContext _context;
        public ContratoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContratoModel>> GetAllAsync()
        {
            return await _context.Contrato.ToListAsync();
        }

        public async Task<ContratoModel?> GetByIdAsync(long idCargo, long idFuncionario)
        {
            return await _context.Contrato
                .FirstAsync(c => c.IdCargo == idCargo && c.IdFuncionario == idFuncionario);
        }

        public async Task<ContratoModel> CreateAsync(ContratoModel contrato)
        {
            _context.Contrato.Add(contrato);
            await _context.SaveChangesAsync();
            return contrato;
        }

        public async Task<ContratoModel> UpdateAsync(ContratoModel contrato)
        {
            _context.Contrato.Update(contrato);
            await _context.SaveChangesAsync();
            return contrato;
        }

        public async Task<bool> DeleteAsync(long idCargo, long idFuncionario)
        {
            var contrato = await GetByIdAsync(idCargo, idFuncionario);
            if (contrato == null)
            {
                return false;
            }

            _context.Contrato.Remove(contrato);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(long idFuncionario, long idCargo)
        {
            return await _context.Contrato.AnyAsync(c => c.IdFuncionario == idFuncionario && c.IdCargo == idCargo);
        }
    }    
}
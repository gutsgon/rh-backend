using Microsoft.EntityFrameworkCore;
using Rh_Backend.Data;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;

namespace Rh_Backend.Repository
{
    public class HistoricoAlteracaoRepository : IHistoricoAlteracaoRepository
    {
        private readonly AppDbContext _context;
        public HistoricoAlteracaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistoricoAlteracaoModel>> GetAllAsync()
        {
            return await _context.HistoricoAlteracao.ToListAsync();
        }

        public async Task<HistoricoAlteracaoModel?> GetByIdAsync(long id)
        {
            return await _context.HistoricoAlteracao.FindAsync(id);
        }

        public async Task<IEnumerable<HistoricoAlteracaoModel?>> SearchByIdAsync(long idFuncionario, long? idCargo, long? idFerias)
        {
            if (idCargo != null && idFerias != null)
            {
                return await _context.HistoricoAlteracao
                    .Where(h => h.IdFuncionario == idFuncionario
                    && h.IdCargo == idCargo
                    && h.IdFerias == idFerias)
                    .ToListAsync();
            }

            return await _context.HistoricoAlteracao
                .Where(h => h.IdFuncionario == idFuncionario)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistoricoAlteracaoModel?>> GetByDataAlteracaoAsync(DateTime dataAlteracao)
        {
            return await _context.HistoricoAlteracao
                .Where(h => h.DataAlteracao.Date <= dataAlteracao.Date)
                .ToListAsync();
        }

        public async Task<HistoricoAlteracaoModel> CreateAsync(HistoricoAlteracaoModel historicoAlteracao)
        {
            _context.HistoricoAlteracao.Add(historicoAlteracao);
            await _context.SaveChangesAsync();
            return historicoAlteracao;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var historicoAlteracao = await GetByIdAsync(id);
            if (historicoAlteracao == null)
            {
                return false;
            }

            _context.HistoricoAlteracao.Remove(historicoAlteracao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.HistoricoAlteracao.AnyAsync(c => c.Id == id);
        }
        
    }
}
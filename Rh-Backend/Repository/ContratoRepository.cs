
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

        public async Task<ContratoModel> UpdateAsync(long idFuncionario, long idCargoAntigo, long idCargoNovo)
        {
            var contratoExistente = await _context.Contrato
                .FirstOrDefaultAsync(c => c.IdFuncionario == idFuncionario && c.IdCargo == idCargoNovo);

            if (contratoExistente != null)
            {
                return contratoExistente;
            }

            var contratoAntigo = await _context.Contrato
                .FirstOrDefaultAsync(c => c.IdFuncionario == idFuncionario && c.IdCargo == idCargoAntigo);

            if (contratoAntigo == null)
            {
                throw new Exception("Contrato antigo não encontrado.");
            }

            _context.Contrato.Remove(contratoAntigo);

            var novoContrato = new ContratoModel
            {
                IdFuncionario = idFuncionario,
                IdCargo = idCargoNovo
            };

            await _context.Contrato.AddAsync(novoContrato);
            await _context.SaveChangesAsync();

            return novoContrato;
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
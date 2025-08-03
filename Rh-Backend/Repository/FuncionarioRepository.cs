using Microsoft.EntityFrameworkCore;
using Rh_Backend.Data;
using Rh_Backend.DTO;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;

namespace Rh_Backend.Repository
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly AppDbContext _context;

        public FuncionarioRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FuncionarioModel>> GetAllAsync()
        {
            return await _context.Funcionarios.ToListAsync();
        }

        public async Task<FuncionarioDetalhesDTO> GetWithDetailsAsync(long id)
        {
            return await _context.Funcionarios.Include(c => c.Contratos)
                    .ThenInclude(ca => ca.Cargo)
                    .Include(fe => fe.Ferias)
                    .Where(f => f.Id == id)
                    .Select(f => new FuncionarioDetalhesDTO
                    {
                        Nome = f.Nome,
                        DataAdmissao = f.DataAdmissao,
                        Salario = f.Salario,
                        Status = f.Status,
                        Cargos = f.Contratos.Select(c => new CargoCreateDTO
                        {
                            Nome = c.Cargo.Nome
                        }).ToList(),
                        Ferias = f.Ferias.Select(fe => new FeriasCreateDTO
                        {
                            IdFuncionario = f.Id,
                            DataInicio = fe.DataInicio,
                            DataFim = fe.DataFim,
                        }).ToList(),

                    }).FirstAsync();
        }

        public async Task<IEnumerable<FuncionarioModel>> SearchAsync(DateTime? dataAdmissao, string nome, decimal salario, bool status)
        {
            if (dataAdmissao.HasValue)
            {
                var funcionarios = await _context.Funcionarios.Where(f => f.DataAdmissao.Date == dataAdmissao.Value.Date
                && f.Nome.Equals(nome)
                && f.Salario == salario
                && f.Status == status).ToListAsync();
                return funcionarios;
            }

            return await _context.Funcionarios.Where(f => f.Nome.Equals(nome) && f.Status == status && f.Salario == salario).ToListAsync();
        }

        public async Task<IEnumerable<FuncionarioModel>> GetByCargoAsync(string cargo)
        {
            var funcionarios = await _context.Contrato
                .Include(f => f.Funcionario)
                .Include(c => c.Cargo)
                .Where(c => c.Cargo.Nome.Equals(cargo))
                .Select(f => f.Funcionario)
                .Distinct()
                .ToListAsync();

            return funcionarios;
        }

        public async Task<FuncionarioModel?> GetByIdAsync(long id)
        {
            return await _context.Funcionarios.FirstAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<FuncionarioComCargoDTO>> GetFuncionariosWithCargo()
        {
            return await _context.Funcionarios
                .Include(c => c.Contratos)
                .ThenInclude(ca => ca.Cargo)
                .Select(f => new FuncionarioComCargoDTO
                {
                    Nome = f.Nome,
                    DataAdmissao = f.DataAdmissao,
                    Salario = f.Salario,
                    Status = f.Status,
                    Cargos = f.Contratos.Select(c => new CargoCreateDTO
                    {
                        Nome = c.Cargo.Nome
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<FuncionarioModel> CreateAsync(FuncionarioModel funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();
            return funcionario;
        }

        public async Task<FuncionarioModel> UpdateAsync(FuncionarioModel funcionario)
        {
            _context.Funcionarios.Update(funcionario);
            await _context.SaveChangesAsync();
            return funcionario;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return false;
            }

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Funcionarios.AnyAsync(c => c.Id == id);
        }
    }
}
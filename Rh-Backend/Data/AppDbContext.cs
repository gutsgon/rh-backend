using Microsoft.EntityFrameworkCore;
using Rh_Backend.Models;

namespace Rh_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<FuncionarioModel> Funcionarios { get; set; }
        public DbSet<CargoModel> Cargo { get; set; }
        public DbSet<FeriasModel> Ferias { get; set; }
        public DbSet<HistoricoAlteracaoModel> HistoricoAlteracao { get; set; }

        public DbSet<ContratoModel> Contrato { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Registro automático de todas as configurações
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
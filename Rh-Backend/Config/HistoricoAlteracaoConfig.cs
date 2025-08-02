using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rh_Backend.Models;

namespace Rh_Backend.Config
{
    public class HistoricoAlteracaoConfig : IEntityTypeConfiguration<HistoricoAlteracaoModel>
    {
        public void Configure(EntityTypeBuilder<HistoricoAlteracaoModel> builder)
        {
            // Campos da tabela
            builder.ToTable("historicoAlteracao");
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.IdFuncionario)
                .IsRequired()
                .HasColumnName("idFuncionario")
                .HasColumnType("bigint");

            builder.Property(h => h.IdFerias)
                .IsRequired()
                .HasColumnName("idFerias")
                .HasColumnType("bigint");
            
            builder.Property(h => h.IdCargo)
                .IsRequired()
                .HasColumnName("idCargo")
                .HasColumnType("bigint");

            builder.Property(h => h.DataAlteracao)
                .IsRequired()
                .HasColumnName("dataAlteracao")
                .HasColumnType("datetime");

            builder.Property(h => h.CampoAlterado)
                .HasColumnName("campoAlterado")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(h => h.ValorAntigo)
                .HasColumnName("valorAntigo")
                .HasColumnType("nvarchar(255)")
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(h => h.ValorNovo)
                .HasColumnName("valorNovo")
                .HasColumnType("nvarchar(255)")
                .IsRequired()
                .HasMaxLength(255);


            // Relacionamentos

            // Funcionario
            builder.HasOne(h => h.Funcionario)
                .WithMany(f => f.HistoricoAlteracao)
                .HasForeignKey(h => h.IdFuncionario)
                .HasConstraintName("FK_FUNCIONARIO_HISTORICO_ALTERACAO")
                .OnDelete(DeleteBehavior.Restrict);

            // Cargo 
            builder.HasOne(h => h.Cargo)
                .WithMany(c => c.HistoricoAlteracao)
                .HasForeignKey(h => h.IdCargo)
                .HasConstraintName("FK_CARGO_HISTORICO_ALTERACAO")
                .OnDelete(DeleteBehavior.Restrict);

            // Ferias
            builder.HasOne(h => h.Ferias)
                .WithMany(f => f.HistoricoAlteracao)
                .HasForeignKey(h => h.IdFerias)
                .HasConstraintName("FK_FERIAS_HISTORICO_ALTERACAO")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
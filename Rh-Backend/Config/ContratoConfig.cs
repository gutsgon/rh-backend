using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rh_Backend.Models;

namespace Rh_Backend.Config
{
    public class ContratoConfig : IEntityTypeConfiguration<ContratoModel>
    {
        public void Configure(EntityTypeBuilder<ContratoModel> builder)
        {
            // Campos da Tabela
            builder.ToTable("contrato");
            builder.HasKey(c => new { c.IdFuncionario, c.IdCargo });

            builder.Property(c => c.IdFuncionario)
                .HasColumnName("idFuncionario")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(c => c.IdCargo)
                .HasColumnName("idCargo")
                .HasColumnType("bigint")
                .IsRequired();

            // Relacionamentos

            // Funcionario
            builder.HasOne(c => c.Funcionario)
                .WithMany(f => f.Contratos)
                .HasForeignKey(c => c.IdFuncionario)
                .HasConstraintName("FK_FUNCIONARIO")
                .OnDelete(DeleteBehavior.Restrict);

            // Cargo
            builder.HasOne(c => c.Cargo)
                .WithMany(c => c.Contratos)
                .HasForeignKey(c => c.IdCargo)
                .HasConstraintName("FK_CARGO")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rh_Backend.Models;

namespace Rh_Backend.Config
{
    public class CargoConfig : IEntityTypeConfiguration<CargoModel>
    {
        public void Configure(EntityTypeBuilder<CargoModel> builder)
        {
            // Campos da tabela
            builder.ToTable("cargo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Nome)
                .HasColumnName("nome")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(c => c.Nome).IsUnique();

            // Relacionamentos 

            // Contratos
            builder.HasMany(f => f.Contratos)
                .WithOne(c => c.Cargo)
                .HasForeignKey(c => c.IdCargo)
                .OnDelete(DeleteBehavior.Restrict);

            // HistoricoAlteracao 
            builder.HasMany(c => c.HistoricoAlteracao)
                .WithOne(h => h.Cargo)
                .HasForeignKey(h => h.IdCargo)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
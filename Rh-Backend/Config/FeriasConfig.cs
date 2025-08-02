using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rh_Backend.Models;

namespace Rh_Backend.Config
{
    public class FeriasConfig : IEntityTypeConfiguration<FeriasModel>
    {
        public void Configure(EntityTypeBuilder<FeriasModel> builder)
        {
            // Campos da Tabela
            builder.ToTable("ferias");
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(f => f.IdFuncionario)
                .HasColumnName("idFuncionario")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(f => f.DataInicio)
                .HasColumnName("dataInicio")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(f => f.DataFim)
                .HasColumnName("dataFim")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(f => f.Status)
                .HasColumnName("status")
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            // Relacionamentos

            // Funcionário 
            builder.HasOne(f => f.Funcionario)
                .WithMany(func => func.Ferias)
                .HasForeignKey(f => f.IdFuncionario)
                .HasConstraintName("FK_FERIAS_FUNCIONARIO")
                .OnDelete(DeleteBehavior.Restrict);


            // HistoricoAlteracao 
            builder.HasMany(f => f.HistoricoAlteracao)
                .WithOne(h => h.Ferias)
                .HasForeignKey(h => h.IdFerias)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
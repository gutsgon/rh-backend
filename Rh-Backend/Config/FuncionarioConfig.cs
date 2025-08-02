using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rh_Backend.Models;

namespace Rh_Backend.Config
{
    public class FuncionarioConfig : IEntityTypeConfiguration<FuncionarioModel>
    {
        public void Configure(EntityTypeBuilder<FuncionarioModel> builder)
        {
            // Campos da Tabela
            builder.ToTable("funcionario");
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(f => f.Nome)
                .HasColumnName("nome")
                .HasColumnType("nvarchar(150)")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(f => f.DataAdmissao)
                .HasColumnName("dataAdmissao")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired()
                .HasColumnType("date");

            builder.Property(f => f.Salario)
                .HasColumnName("salario")
                .HasDefaultValue(0)
                .IsRequired()
                .HasColumnType("numeric(18,2)");

            builder.Property(f => f.Status)
                .HasColumnName("status")
                .HasDefaultValue(false)
                .IsRequired()
                .HasColumnType("bit");


            // Relacionamentos

            // Ferias 
            builder.HasMany(f => f.Ferias)
                .WithOne(fe => fe.Funcionario)
                .HasForeignKey(fe => fe.IdFuncionario)
                .OnDelete(DeleteBehavior.Restrict);

            // Contratos
            builder.HasMany(f => f.Contratos)
                .WithOne(c => c.Funcionario)
                .HasForeignKey(c => c.IdFuncionario)
                .OnDelete(DeleteBehavior.Restrict);

            // Historico de Alterações
            builder.HasMany(f => f.HistoricoAlteracao)
                .WithOne(h => h.Funcionario)
                .HasForeignKey(h => h.IdFuncionario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
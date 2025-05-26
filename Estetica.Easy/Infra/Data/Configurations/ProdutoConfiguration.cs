using Estetica.Easy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estetica.Easy.Infra.Data.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Tempo)
                .IsRequired();

            builder.Property(p => p.Preco)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.CategoriaId)
                .IsRequired();

            builder.ToTable("TB_Produto");

            //builder.HasOne<Categoria>() // Supondo que exista uma classe Categoria
            //    .WithMany()
            //    .HasForeignKey(p => p.CategoriaId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Outras configurações conforme necessário
        }
    }
}

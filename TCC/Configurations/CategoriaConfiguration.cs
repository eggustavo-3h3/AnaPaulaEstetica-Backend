using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;
namespace TCC.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.CategoriaImagem)
                .IsRequired();

            builder.ToTable("TB_Categoria");

            // Outras configurações conforme necessário
        }
    }
}

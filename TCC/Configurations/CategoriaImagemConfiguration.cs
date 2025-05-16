using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Configurations
{
    public class CategoriaImagemConfiguration : IEntityTypeConfiguration<CategoriaImagem>
    {
        public void Configure(EntityTypeBuilder<CategoriaImagem> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Imagem)
                .HasMaxLength(500);

            builder.ToTable("TB_CategoriaImagem");
            // Outras configurações conforme necessário
        }
    }
}

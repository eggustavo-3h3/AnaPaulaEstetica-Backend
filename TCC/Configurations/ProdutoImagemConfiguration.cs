using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Configurations
{
    public class ProdutoImagemConfiguration : IEntityTypeConfiguration<ProdutoImagem>
    {
        public void Configure(EntityTypeBuilder<ProdutoImagem> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Imagem)
                .HasMaxLength(500);

            builder.ToTable("TB_ProdutoImagem");

            // Outras configurações conforme necessário
        }
    }
}

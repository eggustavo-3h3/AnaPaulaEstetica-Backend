using Estetica.Easy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estetica.Easy.Infra.Data.Configurations
{
    public class ProdutoImagemConfiguration : IEntityTypeConfiguration<ProdutoImagem>
    {
        public void Configure(EntityTypeBuilder<ProdutoImagem> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Imagem)
                .IsRequired();

            builder.ToTable("TB_ProdutoImagem");

            // Outras configurações conforme necessário
        }
    }
}

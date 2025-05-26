using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.Configurations
{
    public class CategoriaImagemConfiguration : IEntityTypeConfiguration<CategoriaImagem>
    {
        public void Configure(EntityTypeBuilder<CategoriaImagem> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Imagem)
                .IsRequired();

            builder.ToTable("TB_CategoriaImagem");
            // Outras configurações conforme necessário
        }
    }
}

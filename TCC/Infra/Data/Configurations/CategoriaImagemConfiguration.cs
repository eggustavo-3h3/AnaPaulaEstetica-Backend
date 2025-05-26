using Estetica.Easy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estetica.Easy.Infra.Data.Configurations
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

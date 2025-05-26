using Estetica.Easy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estetica.Easy.Infra.Data.Configurations
{
    public class AgendamentoProdutoConfiguration : IEntityTypeConfiguration<AgendamentoProduto>
    {
        public void Configure(EntityTypeBuilder<AgendamentoProduto> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProdutoId)
                .IsRequired();

            builder.Property(p => p.AgendamentoId)
                .IsRequired();

            builder.ToTable("TB_AgendamentoProduto");

            // Outras configurações conforme necessário
        }
    }
}

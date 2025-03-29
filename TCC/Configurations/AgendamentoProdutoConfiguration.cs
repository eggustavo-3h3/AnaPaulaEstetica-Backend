using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Configurations
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

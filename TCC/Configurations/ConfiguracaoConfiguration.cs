using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;
namespace TCC.Configurations
{
    public class ConfiguracaoConfiguration : IEntityTypeConfiguration<Configuracao>
    {
        public void Configure(EntityTypeBuilder<Configuracao> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.TempoParaCancelamento)
                .IsRequired();

            builder.ToTable("TB_Configuracao");

            // Outras configurações conforme necessário
        }
    }
}

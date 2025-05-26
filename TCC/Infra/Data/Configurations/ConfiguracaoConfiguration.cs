using Estetica.Easy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estetica.Easy.Infra.Data.Configurations
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

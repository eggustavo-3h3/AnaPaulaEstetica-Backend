using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.Configurations
{
    public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DataHoraInicial)
                .IsRequired();

            builder.Property(p => p.DataHoraFinal)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.ToTable("TB_Agendamento");

            // Outras configurações conforme necessário
        }
    }
}

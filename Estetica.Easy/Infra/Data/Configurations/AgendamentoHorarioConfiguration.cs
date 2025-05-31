using Estetica.Easy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estetica.Easy.Infra.Data.Configurations
{
    public class AgendamentoHorarioConfiguration : IEntityTypeConfiguration<AgendamentoHorario>
    {
        public void Configure(EntityTypeBuilder<AgendamentoHorario> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AgendamentoId)
                .IsRequired();

            builder.Property(p => p.Data)
                .IsRequired();

            builder.Property(p => p.Hora)
                .IsRequired();

            builder.ToTable("TB_AgendamentoHorario");
        }
    }
}

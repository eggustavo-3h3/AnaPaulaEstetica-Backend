using Estetica.Easy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estetica.Easy.Infra.Data.Configurations
{
    public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.UsuarioId)
                .IsRequired();

            builder.Property(p => p.ProdutoId)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.ToTable("TB_Agendamento");
        }
    }
}

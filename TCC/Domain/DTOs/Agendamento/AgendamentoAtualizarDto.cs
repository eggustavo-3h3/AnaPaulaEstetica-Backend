using TCC.Domain.Enumerators;

namespace TCC.Domain.DTOs.Agendamento
{
    public class AgendamentoAtualizarDto
    {
        public Guid Id { get; set; }
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public EnumStatus Status { get; set; }
    }
}

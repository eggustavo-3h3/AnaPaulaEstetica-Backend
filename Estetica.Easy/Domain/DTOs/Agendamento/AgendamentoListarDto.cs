using Estetica.Easy.Domain.Enumerators;

namespace Estetica.Easy.Domain.DTOs.Agendamento
{
    public class AgendamentoListarDto
    {
        public Guid Id { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly HoraInicial { get; set; }
        public TimeOnly HoraFinal { get; set; }
        public EnumStatus Status { get; set; }
    }
}

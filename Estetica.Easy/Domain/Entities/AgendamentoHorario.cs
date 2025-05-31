namespace Estetica.Easy.Domain.Entities
{
    public class AgendamentoHorario
    {
        public Guid Id { get; set; }
        public Guid AgendamentoId { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Hora { get; set; }

        #region Propriedades de Navegabilidade

        public Agendamento Agendamento { get; set; } = null!;

        #endregion

    }
}

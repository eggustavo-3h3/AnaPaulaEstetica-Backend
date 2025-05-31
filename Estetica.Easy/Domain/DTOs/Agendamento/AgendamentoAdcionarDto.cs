namespace Estetica.Easy.Domain.DTOs.Agendamento
{
    public class AgendamentoAdcionarDto
    {
        public Guid ProdutoId { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Hora { get; set; }
    }
}

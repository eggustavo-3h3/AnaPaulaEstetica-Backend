namespace Estetica.Easy.Domain.DTOs.Agendamento;

public class AgendamentoHorarioDisponivel(TimeOnly horario, bool ocupado)
{
    public TimeOnly Horario { get; set; } = horario;
    public bool Ocupado { get; set; } = ocupado;
}

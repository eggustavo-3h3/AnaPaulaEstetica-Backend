using Estetica.Easy.Domain.Enumerators;

namespace Estetica.Easy.Domain.Entities
{
    public class Agendamento
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ProdutoId { get; set; }
        public EnumStatus Status { get; set; } = EnumStatus.Agendado;
        public List<AgendamentoHorario> Horarios { get; set; } = [];

        #region Propriedade de Navegabilidde

        public Usuario Usuario { get; set; } = null!;
        public Produto Produto { get; set; } = null!;

        #endregion
    }
}

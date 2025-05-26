using Estetica.Easy.Domain.Enumerators;

namespace Estetica.Easy.Domain.Entities
{
    public class Agendamento
    {
        public Guid Id { get; set; }
        public Guid Usuarioid { get; set; }
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public EnumStatus Status { get; set; }

        #region Propriedade de Navegabilidde
        public Usuario Usuario { get; set; }
        #endregion
    }
}

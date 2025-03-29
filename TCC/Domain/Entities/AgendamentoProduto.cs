namespace TCC.Domain.Entities
{
    public class AgendamentoProduto
    {
        public Guid Id { get; set; }
        public Guid AgendamentoId { get; set; }
        public Guid ProdutoId { get; set; }

        #region Propriedades de Navegabilidade

        public Agendamento Agendamento { get; set; }
        public Produto Produto { get; set; }


        #endregion

    }
}

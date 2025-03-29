namespace TCC.Domain.Entities
{
    public class ProdutoImagem
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string Imagem { get; set; } = string.Empty;

        #region Propriedades de Navegabilidade
        public Produto Produto { get; set; }
        #endregion
    }
}

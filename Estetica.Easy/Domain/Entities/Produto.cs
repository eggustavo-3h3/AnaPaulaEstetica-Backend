using Estetica.Easy.Domain.Enumerators;

namespace Estetica.Easy.Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public Guid CategoriaId { get; set; }
        public EnumTempo Tempo { get; set; }
        public decimal Preco { get; set; }
        public ICollection<ProdutoImagem> ProdutoImagens { get; set; }

        #region Propriedades de Navegabilidade
        public Categoria Categoria { get; set; }

        #endregion
    }
}

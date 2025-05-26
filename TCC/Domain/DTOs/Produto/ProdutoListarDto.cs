using TCC.Domain.Enumerators;

namespace TCC.Domain.DTOs.Produto
{
    public class ProdutoListarDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public Guid CategoriaId { get; set; }
        public string CategoriaDescricao { get; set; } = string.Empty;
        public EnumTempo Tempo { get; set; }
        public decimal Preco { get; set; }
        public ICollection<Entities.ProdutoImagem> ProdutoImagens { get; set; }
    }
}

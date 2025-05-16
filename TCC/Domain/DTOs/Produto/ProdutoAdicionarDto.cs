using TCC.Domain.DTOs.ProdutoImagem;
using TCC.Domain.Enumerators;

namespace TCC.Domain.DTOs.Produto
{
    public class ProdutoAdicionarDto
    {
        public string Descricao { get; set; } = string.Empty;
        public EnumTempo Tempo { get; set; }
        public decimal Preco { get; set; }
        public List<ProdutoImagemAdicionarDto> Imagens { get; set; }
        public Guid CategoriaId { get; set; }
    }
}

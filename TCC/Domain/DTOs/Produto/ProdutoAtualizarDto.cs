using TCC.Domain.DTOs.ProdutoImagem;
using TCC.Domain.Enumerators;

namespace TCC.Domain.Dtos
{
    public class ProdutoAtualizarDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public EnumTempo Tempo { get; set; }
        public decimal Preco { get; set; }
        public List<ProdutoImagemAdicionarDto> Imagens { get; set; }
    }
}

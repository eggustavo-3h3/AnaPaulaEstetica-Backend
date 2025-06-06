using Estetica.Easy.Domain.Enumerators;

namespace Estetica.Easy.Domain.DTOs.Produto
{
    public class ProdutoAtualizarDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public EnumTempo Tempo { get; set; }
        public decimal Preco { get; set; }
        public string Imagem { get; set; }

        public Guid CategoriaId { get; set; }
    }
}

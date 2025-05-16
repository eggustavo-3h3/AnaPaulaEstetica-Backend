using TCC.Domain.DTOs.CategoriaImagem;
using TCC.Domain.DTOs.ProdutoImagem;

namespace TCC.Domain.Entities
{
    public class CategoriaImagem
    {
        public Guid Id { get; set; }
        public string Imagem { get; set; } = string.Empty;
    }
}

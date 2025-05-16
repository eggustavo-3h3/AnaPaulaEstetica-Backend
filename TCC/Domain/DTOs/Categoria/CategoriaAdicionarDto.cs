using TCC.Domain.DTOs.CategoriaImagem;

namespace TCC.Domain.DTOs.Categoria
{
    public class CategoriaAdicionarDto
    {
        public string Descricao { get; set; }

        public List<CategoriaImagemAdicionarDto> Imagens { get; set; }
    }
}

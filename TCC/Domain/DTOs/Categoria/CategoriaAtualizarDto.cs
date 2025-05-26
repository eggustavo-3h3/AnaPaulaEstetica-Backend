namespace TCC.Domain.DTOs.Categoria
{
    public class CategoriaAtualizarDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }

        public string CategoriaImagem { get; set; } = string.Empty;
    }
}

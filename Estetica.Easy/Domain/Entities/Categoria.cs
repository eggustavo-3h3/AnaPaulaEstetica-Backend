﻿namespace Estetica.Easy.Domain.Entities
{
    public class Categoria
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string CategoriaImagem { get; set; }
    }
}

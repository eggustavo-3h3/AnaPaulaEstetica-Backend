using FluentValidation;

namespace TCC.Domain.DTOs.Produto
{
    public class ProdutoAdicionarDtoValidator : AbstractValidator<ProdutoAdicionarDto>
    {
        public ProdutoAdicionarDtoValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty()
                .WithMessage("A descrição é obrigatória.")
                .MaximumLength(500)
                .WithMessage("A descrição deve ter no máximo 500 caracteres.");
            RuleFor(x => x.Tempo)
                .IsInEnum()
                .WithMessage("O tempo deve ser um valor válido.");
            RuleFor(x => x.Preco)
                .GreaterThan(0)
                .WithMessage("O preço deve ser maior que zero.");
            RuleFor(x => x.CategoriaId)
                .NotEmpty()
                .WithMessage("A categoria é obrigatória.");
        }
    }     
}
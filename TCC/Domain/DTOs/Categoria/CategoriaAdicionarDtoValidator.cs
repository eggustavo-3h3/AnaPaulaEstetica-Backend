using FluentValidation;

namespace TCC.Domain.DTOs.Categoria
{
    public class CategoriaAdicionarDtoValidator : AbstractValidator<CategoriaAdicionarDto>
    {
        public CategoriaAdicionarDtoValidator()
        {
            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("O campo descrição é obrigatório.")
                .MaximumLength(100).WithMessage("O campo descrição deve ter no máximo 100 caracteres.");
        }
    } 
}

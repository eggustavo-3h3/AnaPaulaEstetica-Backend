using FluentValidation;
using TCC.Domain.Dtos;

namespace TCC.Domain.DTOs.Categoria
{
    public class CategoriaAtualizarDtoValidator : AbstractValidator<CategoriaAtualizarDto>
    {
        public CategoriaAtualizarDtoValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O campo Id é obrigatório.");
            RuleFor(c => c.Descricao)
                .NotEmpty()
                .WithMessage("O campo Descrição é obrigatório.")
                .Length(1, 100)
                .WithMessage("O campo Descrição deve ter até 100 caracteres.");
        }
    }
}

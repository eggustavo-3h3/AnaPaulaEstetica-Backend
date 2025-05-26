using FluentValidation;

namespace Estetica.Easy.Domain.DTOs.Usuario
{
    public class UsuarioAtualizarDtoValidator : AbstractValidator<UsuarioAtualizarDto>
    {
        public UsuarioAtualizarDtoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O nome é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O nome deve ter no máximo 100 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O email é obrigatório.")
                .EmailAddress()
                .WithMessage("O email deve ser um endereço de email válido.");
        }
    }    
}

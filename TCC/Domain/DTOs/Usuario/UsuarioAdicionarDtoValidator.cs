using FluentValidation;

namespace TCC.Domain.DTOs.Usuario
{
    public class UsuarioAdicionarDtoValidator : AbstractValidator<UsuarioAdicionarDto>
    {
        public UsuarioAdicionarDtoValidator()
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
            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("A senha é obrigatória.")
                .MinimumLength(8)
                .WithMessage("A senha deve ter pelo menos 8 caracteres.");
            RuleFor(x => x.ConfirmarSenha)
                .NotEmpty()
                .WithMessage("A confirmação da senha é obrigatória.")
                .Equal(x => x.Senha).WithMessage("As senhas não coincidem.");
        }
    }
}

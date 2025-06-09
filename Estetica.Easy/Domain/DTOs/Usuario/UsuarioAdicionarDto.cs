using Estetica.Easy.Domain.Enumerators;

namespace Estetica.Easy.Domain.DTOs.Usuario
{
    public class UsuarioAdicionarDto
    {
        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public EnumPerfil Perfil { get; set; }

        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}

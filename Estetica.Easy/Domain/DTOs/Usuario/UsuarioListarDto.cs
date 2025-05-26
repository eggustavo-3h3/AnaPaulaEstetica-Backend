using Estetica.Easy.Domain.Enumerators;

namespace Estetica.Easy.Domain.DTOs.Usuario
{
    public class UsuarioListarDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public EnumPerfil Perfil { get; set; }
    }
}

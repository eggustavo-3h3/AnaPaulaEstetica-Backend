using TCC.Domain.Enumerators;

namespace TCC.Domain.DTOs.Usuario
{
    public class UsuarioListarDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public EnumPerfil Perfil { get; set; }
    }
}

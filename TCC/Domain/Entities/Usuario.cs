﻿using TCC.Domain.Enumerators;

namespace TCC.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EnumPerfil Perfil { get; set; }
        public string Senha { get; set; } = string.Empty;
        public Guid? ChaveResetSenha { get; set; }
    }
}

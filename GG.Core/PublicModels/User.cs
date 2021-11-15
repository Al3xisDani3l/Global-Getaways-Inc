﻿

using System;

namespace GG.Core
{
    public class User : BaseIdentityUser
    {

        
        
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public UserKind TipoUsuario { get; set; }
        public RoleType Role { get; set; }
        public bool Suscrito { get; set; }
        public bool EsVoluntario { get; set; }
        public Guid? GoogleUUID { get; set; }


    }
}

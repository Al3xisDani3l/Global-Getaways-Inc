using System;
using System.Collections.Generic;
using System.Text;

namespace GG.Core
{
    /// <summary>
    /// Representa el perfil de mapeo de todas las entidades con sus respectivos DTO.
    /// </summary>
    public class AutomapperProfile
    {
        public string Namespace { get; set; }

        public Dictionary<string, string> FromTo { get; set; }

        public Dictionary<string, string> SpecialFromTo { get; set; }

    }
}

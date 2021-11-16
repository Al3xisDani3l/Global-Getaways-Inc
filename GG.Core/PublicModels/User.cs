

using System;
using System.ComponentModel.DataAnnotations;

namespace GG.Core
{
    public class User : BaseIdentityUser
    {



        [StringLength(64)]
        public string Name { get; set; }
        [StringLength(64)]
        public string Lastname { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public UserKind? KindUser { get; set; }

        public string? Gender { get; set; }


    }
}

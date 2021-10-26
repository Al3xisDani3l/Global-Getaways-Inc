

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GG.Core
{
    [Table("users")]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Id))]
    public class PrivateUser : BaseEntity 
    {
        public PrivateUser()
        {
            Ordenes = new HashSet<Orden>();

            Comments = new HashSet<OrdenComment>();


        }

      
       
        [StringLength(64), Required]
        public string Name { get; set; }
        [StringLength(64), Required]
        public string Lastname { get; set; }
        [StringLength(10), Required]
        public string Phone { get; set; }
        [DataType(DataType.Date), Required]
        public DateTime Birthday { get; set; }
        [Required, RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public string Email { get; set; }

        [Required]
        public UserKind KindUser { get; set; }

        [Required]
        public RoleType Role { get; set; }

  
        [Required, StringLength(128)]
        public string Password { get; set; }

        public Guid? GoogleUUID { get; set; }

        [Required]
        public virtual ICollection<Orden> Ordenes { get; set; }
        [Required]
        public virtual ICollection<OrdenComment> Comments { get; set; }
    }
}

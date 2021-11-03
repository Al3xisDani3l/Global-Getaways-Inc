

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GG.Core
{
    [Table("users")]
    [Microsoft.EntityFrameworkCore.Index(nameof(Email), IsUnique = true)]
    [Microsoft.EntityFrameworkCore.Index(nameof(Id))]

    public class PrivateUser : BaseIdentityUser
    {
       


        public PrivateUser()
        {
         //   Ordenes = new HashSet<Orden>();

          //  Comments = new HashSet<OrdenComment>();


        }

      
       
        [StringLength(64)]
        public string Name { get; set; }
        [StringLength(64)]
        public string Lastname { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
    
        public UserKind? KindUser { get; set; }

        public string? Gender { get; set; }

        public string? UsernameGoogle { get; set; }
       
     //   public virtual ICollection<Orden> Ordenes { get; set; }
       
      //  public virtual ICollection<OrdenComment> Comments { get; set; }
    }
}

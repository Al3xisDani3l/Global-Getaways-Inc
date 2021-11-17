

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
            MyLikes = new HashSet<LikedPackage>();

            MyRatings = new HashSet<PrivateRating>();


        }

        [StringLength(64)]
        public string Name { get; set; }
        [StringLength(64)]
        public string Lastname { get; set; }
        [DataType(DataType.Date)]

        public string ProfileImg { get; set; }

        public DateTime? Birthday { get; set; }
    
        public string? Gender { get; set; }

       
        public virtual ICollection<LikedPackage> MyLikes { get; set; }
       
       public virtual ICollection<PrivateRating> MyRatings { get; set; }
    }
}

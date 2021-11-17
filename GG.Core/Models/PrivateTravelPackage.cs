using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GG.Core
{
    public class PrivateTravelPackage: BaseEntity, IRemarkable
    {


       public PrivateTravelPackage()
        {
            Ratings = new HashSet<PrivateRating>();
            Likes = new HashSet<LikedPackage>();
        }


        [StringLength(512)]
        public string Review { get; set; }

        public string PathingImage { get; set; }

        public string Labels { get; set; }

        public string? State { get; set; }
        public string Country { get; set; }

        public string NamePackage { get; set; }
        public bool IsAvailable { get; set; } = true;

        [Range(0,1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [NotMapped]
        public double Punctuation { get {

                if (Ratings.Count > 0)
                {

                   return Ratings.Average(r => r.Punctuation);


                }
                return 0;
            
            } }

        [NotMapped]
        public int MyLikes
        {
            get
            {
                return Likes.Count;
            }
        }


        public virtual ICollection<PrivateRating> Ratings { get; set; }

        public virtual ICollection<LikedPackage> Likes { get; set; }


    }
}

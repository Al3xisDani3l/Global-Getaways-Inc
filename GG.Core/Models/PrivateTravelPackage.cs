using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GG.Core
{
    public class PrivateTravelPackage: BaseEntity, IRemarkable
    {
        [StringLength(512)]
        public string Review { get; set; }

        public string PathingImage { get; set; }

        public string Labels { get; set; }

        public string Country { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Range(0,1000000)]
        public decimal Price { get; set; }


        public virtual ICollection<PrivateRating> Ratings { get; set; }

        public virtual ICollection<LikedPackage> Likeds { get; set; }


    }
}

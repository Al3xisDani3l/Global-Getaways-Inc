using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GG.Core
{
    public class PrivateRating: BaseEntity
    {
        [ForeignKey("IdUserNavigation")]
        public string UserId { get; set; }

        [ForeignKey("IdTravelPackageNavigation")]
        public int TravelPackageId { get; set; }

        [Range(1,5)]
        public int Punctuation { get; set; }

        [StringLength(255)]
        public string Review { get; set; }

        public DateTime PostingDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public virtual PrivateUser IdUserNavigation { get; set; }
        
        public virtual PrivateTravelPackage IdTravelPackageNavigation { get; set; }

    }
}

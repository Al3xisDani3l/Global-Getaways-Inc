using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GG.Core
{
    public class LikedPackage:BaseEntity
    {
        [ForeignKey("IdUserNavigation")]
        public string UserId { get; set; }

        [ForeignKey("IdTravelPackageNavigation")]
        public int TravelPackageId { get; set; }

        public virtual PrivateUser IdUserNavigation { get; set; }

        public virtual PrivateTravelPackage IdTravelPackageNavigation { get; set; }

    }
}

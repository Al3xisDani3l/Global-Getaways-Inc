using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace GG.Core
{
    public class PrivateRating: BaseEntity
    {
        [ColumnName(@"UserId")]
        [ForeignKey("IdUserNavigation")]
        public string UserId { get; set; }

        [ColumnName(@"IdTravelPackage")]
        [ForeignKey("IdTravelPackageNavigation")]
        public int TravelPackageId { get; set; }

        [ColumnName(@"Punctuation")]
        [Range(1,5)]
        public int Punctuation { get; set; }

        [ColumnName(@"Comment")]
        [StringLength(255)]
        public string Comment { get; set; }

        [ColumnName(@"PostingDate")]
        public DateTime PostingDate { get; set; } = DateTime.Now;

        [ColumnName(@"LastUpdate")]
        public DateTime LastUpdate { get; set; } = DateTime.Now;

        public virtual PrivateUser IdUserNavigation { get; set; }
        
        public virtual PrivateTravelPackage IdTravelPackageNavigation { get; set; }

    }
}

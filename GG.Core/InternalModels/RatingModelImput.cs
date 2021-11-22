using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GG.Core
{
     public class RatingModelImput
    {

        [ColumnName(@"Id")]
        public float Id { get; set; }

        [ColumnName(@"UserId")]
        public string UserId { get; set; }

        [ColumnName(@"TravelPackageId")]
        public float TravelPackageId { get; set; }

        [ColumnName(@"Punctuation")]
        public float Punctuation { get; set; }

        [ColumnName(@"Comment")]
        public string Comment { get; set; }

        [ColumnName(@"PostingDate")]
        public string PostingDate { get; set; }

        [ColumnName(@"LastUpdate")]
        public string LastUpdate { get; set; }

        [ColumnName(@"Guid")]
        public string Guid { get; set; }
    }
}

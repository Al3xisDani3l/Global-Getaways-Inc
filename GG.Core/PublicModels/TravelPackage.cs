using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GG.Core
{
    public class TravelPackage: BaseEntity
    {

        [StringLength(512)]
        public string Review { get; set; }

        public string PathingImage { get; set; }

        public string Labels { get; set; }

        public string Country { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Range(0, 1000000)]
        public decimal Price { get; set; }
        
        public double Punctuation { get; set; }
       

        public int MyLikes { get; set; }




    }
}

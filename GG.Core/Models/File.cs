
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GG.Core
{
    [Table("files")]
    public class File : BaseEntity
    {
    

        [Required, StringLength(128)]
        public string UriString { get; set; }

        public string Extencion { get; set; }

        public long SizeFile { get; set; }

        public string FileName { get; set; }




        [NotMapped]
        public Uri UriFile { get { return new Uri(UriString) {}; } set { UriString = value.AbsolutePath; } }


     

    }
}

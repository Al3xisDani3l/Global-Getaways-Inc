

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace GG.Core
{
    [Table("ordenes")]
    [Index(nameof(Id), IsUnique = true)]
    public class Orden : BaseEntity
    {
        public Orden()
        {
            Comments = new HashSet<OrdenComment>();
            Files = new HashSet<File>();
        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("IdUserNavigation")]
        public int IdUser { get; set; }
        [Required]
        public double Ancho { get; set; }
        [Required]
        public double Alto { get; set; }
        [StringLength(64)]
        public string MaterialBarda { get; set; }
        [StringLength(128)]
        public string Localizacion { get; set; }
        [StringLength(516)]
        public string Tematica { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaRealizacionDeseada { get; set; }
        [Required]
        public Organization Organizacion { get; set; }

        [NotMapped]
        public string Archivo
        {
            get
            {

                if (Files.Count > 0)
                {
                    return string.Concat("/orders/images/",Files.FirstOrDefault().FileName);
                }
                else
                {
                    return "";
                }


            }
        }


        public virtual PrivateUser IdUserNavigation { get; set; }

        [Required]
        public virtual ICollection<OrdenComment> Comments { get; set; }
        [Required]
        public virtual ICollection<File> Files { get; set; }

    }
}

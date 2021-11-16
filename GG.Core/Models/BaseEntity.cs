using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GG.Core
{
    /// <summary>
    /// Representa la identificacion de una entidad,
    /// esta clase no se puede instanciar.
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();

    }
}

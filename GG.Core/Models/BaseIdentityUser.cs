using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GG.Core
{
    public abstract class BaseIdentityUser : IdentityUser, IEntity<string>
    {

        public Guid Guid { get; set; }
    }
}

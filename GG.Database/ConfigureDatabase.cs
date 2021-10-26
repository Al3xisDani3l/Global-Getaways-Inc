using GG.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GG.Database
{
    public static class ConfigureDatabase
    {

        public static ModelBuilder ConfigureEntities(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PrivateUser>(u => u.ToTable("Users"));

            return modelBuilder;

        }

    }
}

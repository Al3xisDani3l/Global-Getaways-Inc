using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using GG.Core;

namespace GG.WebPage.Data
{
    public class ApplicationDbContext : IdentityDbContext<PrivateUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           builder.Entity<PrivateUser>().ToTable("Users").Property(p => p.Id).HasColumnName("Id");
           builder.Entity<IdentityUserLogin<string>>().ToTable("Logins");
           builder.Entity<IdentityUserClaim<string>>().ToTable("Claims");
           builder.Entity<IdentityRole>().ToTable("Roles");
            


            base.OnModelCreating(builder);
        }


    }
}

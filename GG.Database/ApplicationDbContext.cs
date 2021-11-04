using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using GG.Core;

namespace GG.Data
{
    public class ApplicationDbContext : IdentityDbContext<PrivateUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<PrivateTravelPackage> TravelPackages { get; set; }

        public virtual DbSet<PrivateRating> Ratings { get; set; }

        public virtual DbSet<LikedPackage> LikedPackages { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PrivateUser>().ToTable("Users").Property(p => p.Id).HasColumnName("Id");
            builder.Entity<IdentityUserLogin<string>>().ToTable("Logins");
            builder.Entity<IdentityUserClaim<string>>().ToTable("Claims");
            builder.Entity<IdentityRole>().ToTable("Roles");



            
        }


    }
}

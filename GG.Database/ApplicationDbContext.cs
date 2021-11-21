using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using GG.Core;
using Microsoft.EntityFrameworkCore.Proxies;

namespace GG.Data
{

    public class ApplicationDbContext : IdentityDbContext<PrivateUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<PrivateTravelPackage> TravelPackages { get; set; }

        public virtual DbSet<PrivateRating> Ratings { get; set; }

        public virtual DbSet<LikedPackage> LikedPackages { get; set; }

        public virtual DbSet<ShoppingCart<PrivateTravelPackage>> ShoppingCarts { get; set; }

        public virtual DbSet<CartItem<PrivateTravelPackage>> CartItems { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PrivateUser>().ToTable("Users").Property(p => p.Id).HasColumnName("Id");
            builder.Entity<IdentityUserLogin<string>>().ToTable("Logins");
            builder.Entity<IdentityUserClaim<string>>().ToTable("Claims");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<PrivateTravelPackage>().ToTable("TravelPackages");
            builder.Entity<PrivateRating>().ToTable("Ratings");
            builder.Entity<LikedPackage>().ToTable("Likeds");
           



            
        }


    }
}

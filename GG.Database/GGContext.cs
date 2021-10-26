using Microsoft.EntityFrameworkCore;
using System;
using GG.Core;
namespace GG.Database
{
   
        public partial class GGContext : DbContext
        {
            public GGContext()
            {

            }

            public GGContext(DbContextOptions<GGContext> options)
                : base(options)
            {
            }



            public virtual DbSet<PrivateUser> Users { get; set; }


            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
            
            }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureEntities();
        }



    }
}

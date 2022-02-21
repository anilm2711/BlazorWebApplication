using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class ECommDataBaseContext : DbContext
    {
        public ECommDataBaseContext()
        {
        }

        public ECommDataBaseContext(DbContextOptions<ECommDataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

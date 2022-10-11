using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace _3TeamProject.Models
{
    public partial class _3TeamProjectContext : DbContext
    {
        public _3TeamProjectContext()
        {
        }

        public _3TeamProjectContext(DbContextOptions<_3TeamProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=3TeamProject;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.AdministratorId).HasColumnName("Administrator\r\nID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.ShipAddress)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShipCity)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShipCountry)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.ShipPostalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ShipPostal code");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.AddedTime).HasColumnType("datetime");

                entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategoryID");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.ProductStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProductUnitPrice).HasColumnType("money");

                entity.Property(e => e.QuantityPerUnit).HasMaxLength(50);

                entity.Property(e => e.RemovedTime).HasColumnType("datetime");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

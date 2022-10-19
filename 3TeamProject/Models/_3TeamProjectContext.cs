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

        public virtual DbSet<ActivitiesMessageBoard> ActivitiesMessageBoards { get; set; } = null!;
        public virtual DbSet<Administrator> Administrators { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderStatusCategory> OrderStatusCategories { get; set; } = null!;
        public virtual DbSet<PaymentStatusCategory> PaymentStatusCategories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<ProductsMessageBoard> ProductsMessageBoards { get; set; } = null!;
        public virtual DbSet<ProductsPictureInfo> ProductsPictureInfos { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ShipStatusCategory> ShipStatusCategories { get; set; } = null!;
        public virtual DbSet<Sightseeing> Sightseeings { get; set; } = null!;
        public virtual DbSet<SightseeingMessageBoard> SightseeingMessageBoards { get; set; } = null!;
        public virtual DbSet<SightseeingPictureInfo> SightseeingPictureInfos { get; set; } = null!;
        public virtual DbSet<SocialActivity> SocialActivities { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=3TeamProject;user id=sa;password=Tgm102");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivitiesMessageBoard>(entity =>
            {
                entity.HasKey(e => e.ActivitiesMessageId);

                entity.ToTable("Activities\nMessageBoard");

                entity.Property(e => e.ActivitiesMessageId).HasColumnName("ActivitiesMessageID");

                entity.Property(e => e.ActivitiesCreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ActivitiesId).HasColumnName("ActivitiesID");

                entity.Property(e => e.ActivitiesMessageState)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Activities)
                    .WithMany(p => p.ActivitiesMessageBoards)
                    .HasForeignKey(d => d.ActivitiesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activities\nMessageBoard_SocialActivities");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ActivitiesMessageBoards)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activities\nMessageBoard_Members");
            });

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.HasKey(e => e.AdministratorId)
                    .HasName("PK_Adminstrators");

                entity.Property(e => e.AdministratorId).HasColumnName("AdministratorID");

                entity.Property(e => e.AdministratorName).HasMaxLength(50);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Administrators)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_Adminstrators_Users");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Age)
                    .HasColumnName("age")
                    .HasComputedColumnSql("(datediff(year,[Birthday],getdate()))", false);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CellPhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdentityNumber).HasMaxLength(50);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.Property(e => e.NickName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_Members_Users");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.AdministratorId).HasColumnName("AdministratorID");

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
                    .IsUnicode(false);

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AdministratorId)
                    .HasConstraintName("FK_Orders_Administrators");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Members");

                entity.HasOne(d => d.OrderStatusNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_OrderStatus\nCategories");

                entity.HasOne(d => d.PaymentStatusNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_PaymentStatus\nCategories");

                entity.HasOne(d => d.ShipStatusNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_ShipStatus\nCategories");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Products");
            });

            modelBuilder.Entity<OrderStatusCategory>(entity =>
            {
                entity.HasKey(e => e.OrderCategoryId);

                entity.ToTable("OrderStatus\nCategories");

                entity.Property(e => e.OrderCategoryId).HasColumnName("OrderCategoryID");

                entity.Property(e => e.OrderCategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentStatusCategory>(entity =>
            {
                entity.HasKey(e => e.PaymentCategoryId);

                entity.ToTable("PaymentStatus\nCategories");

                entity.Property(e => e.PaymentCategoryId).HasColumnName("PaymentCategoryID");

                entity.Property(e => e.PaymentCategoryName).HasMaxLength(50);
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

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_ProductCategories");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Products_Suppliers");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<ProductsMessageBoard>(entity =>
            {
                entity.HasKey(e => e.MessageBoardId);

                entity.ToTable("Products\nMessageBoard");

                entity.Property(e => e.MessageBoardId).HasColumnName("MessageBoardID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MessageCreatedTime).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductMessageContent)
                    .HasMaxLength(50)
                    .HasColumnName("ProductMessage\nContent");

                entity.Property(e => e.ProductMessageStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ProductsMessageBoards)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products\nMessageBoard_Members");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductsMessageBoards)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products\nMessageBoard_Products");
            });

            modelBuilder.Entity<ProductsPictureInfo>(entity =>
            {
                entity.HasKey(e => e.ProductPictureId);

                entity.ToTable("Products\nPictureInfo");

                entity.Property(e => e.ProductPictureId).HasColumnName("ProductPictureID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductPictureName).HasMaxLength(50);

                entity.Property(e => e.ProductPicturePath).HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductsPictureInfos)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products\nPictureInfo_Products");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<ShipStatusCategory>(entity =>
            {
                entity.HasKey(e => e.ShipCategoryId);

                entity.ToTable("ShipStatus\nCategories");

                entity.Property(e => e.ShipCategoryId).HasColumnName("ShipCategoryID");

                entity.Property(e => e.ShipCategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<Sightseeing>(entity =>
            {
                entity.ToTable("Sightseeing");

                entity.Property(e => e.SightseeingId).HasColumnName("SightseeingID");

                entity.Property(e => e.AdministratorId).HasColumnName("AdministratorID");

                entity.Property(e => e.SightseeingAddress).HasMaxLength(50);

                entity.Property(e => e.SightseeingCity).HasMaxLength(50);

                entity.Property(e => e.SightseeingCountry).HasMaxLength(50);

                entity.Property(e => e.SightseeingName).HasMaxLength(50);

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Sightseeings)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sightseeing_Administrators");
            });

            modelBuilder.Entity<SightseeingMessageBoard>(entity =>
            {
                entity.HasKey(e => e.MessageBoardId);

                entity.ToTable("Sightseeing\nMessageBoard");

                entity.Property(e => e.MessageBoardId).HasColumnName("MessageBoardID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MessageCreatedTime).HasColumnType("datetime");

                entity.Property(e => e.SightseeingId).HasColumnName("SightseeingID");

                entity.HasOne(d => d.Sightseeing)
                    .WithMany(p => p.SightseeingMessageBoards)
                    .HasForeignKey(d => d.SightseeingId)
                    .HasConstraintName("FK_Sightseeing\nMessageBoard_Sightseeing");
            });

            modelBuilder.Entity<SightseeingPictureInfo>(entity =>
            {
                entity.HasKey(e => e.SightseeingPictureId);

                entity.ToTable("SightseeingPictureInfo");

                entity.Property(e => e.SightseeingPictureId).HasColumnName("SightseeingPictureID");

                entity.Property(e => e.SightseeingId).HasColumnName("SightseeingID");

                entity.Property(e => e.SightseeingPictureName).HasMaxLength(50);

                entity.Property(e => e.SightseeingPicturePath).HasMaxLength(50);

                entity.HasOne(d => d.Sightseeing)
                    .WithMany(p => p.SightseeingPictureInfos)
                    .HasForeignKey(d => d.SightseeingId)
                    .HasConstraintName("FK_SightseeingPictureInfo_Sightseeing");
            });

            modelBuilder.Entity<SocialActivity>(entity =>
            {
                entity.HasKey(e => e.ActivitiesId);

                entity.Property(e => e.ActivitiesId).HasColumnName("ActivitiesID");

                entity.Property(e => e.ActitiesFinishDate).HasColumnType("datetime");

                entity.Property(e => e.ActitiesStartDate).HasColumnType("datetime");

                entity.Property(e => e.ActivitiesAddress).HasMaxLength(50);

                entity.Property(e => e.ActivitiesName).HasMaxLength(50);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.SocialActivities)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SocialActivities_Members");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SuppliersId);

                entity.Property(e => e.SuppliersId).HasColumnName("SuppliersID");

                entity.Property(e => e.CellPhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName).HasMaxLength(50);

                entity.Property(e => e.ContactName).HasMaxLength(50);

                entity.Property(e => e.Fax)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SupplierAddress).HasMaxLength(50);

                entity.Property(e => e.SupplierCity).HasMaxLength(20);

                entity.Property(e => e.SupplierCountry).HasMaxLength(20);

                entity.Property(e => e.SupplierPhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierPostalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxId).HasColumnName("TaxID");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_Suppliers_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.PasswordHash).HasMaxLength(128);

                entity.Property(e => e.PasswordResetToken)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt).HasMaxLength(128);

                entity.Property(e => e.ResetTokenExpires).HasColumnType("datetime");

                entity.Property(e => e.VerficationToken)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.VerfiedAt).HasColumnType("datetime");

                entity.HasOne(d => d.RolesNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Roles)
                    .HasConstraintName("FK_Users_Roles1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

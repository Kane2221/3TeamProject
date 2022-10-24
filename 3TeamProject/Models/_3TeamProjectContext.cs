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
        public virtual DbSet<AdministratorStatusCategory> AdministratorStatusCategories { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<MemberStatusCategory> MemberStatusCategories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderStatusCategory> OrderStatusCategories { get; set; } = null!;
        public virtual DbSet<PaymentStatusCategory> PaymentStatusCategories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<ProductStatusCategoy> ProductStatusCategoys { get; set; } = null!;
        public virtual DbSet<ProductsMessageBoard> ProductsMessageBoards { get; set; } = null!;
        public virtual DbSet<ProductsPictureInfo> ProductsPictureInfos { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ShipStatusCategory> ShipStatusCategories { get; set; } = null!;
        public virtual DbSet<Sightseeing> Sightseeings { get; set; } = null!;
        public virtual DbSet<SightseeingCategory> SightseeingCategories { get; set; } = null!;
        public virtual DbSet<SightseeingMessageBoard> SightseeingMessageBoards { get; set; } = null!;
        public virtual DbSet<SightseeingPictureInfo> SightseeingPictureInfos { get; set; } = null!;
        public virtual DbSet<SightseeingStatusCategory> SightseeingStatusCategories { get; set; } = null!;
        public virtual DbSet<SocialActivity> SocialActivities { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<SupplierStatusCategoy> SupplierStatusCategoys { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivitiesMessageBoard>(entity =>
            {
                entity.HasKey(e => e.ActivitiesMessageId)
                    .HasName("PK_Activities\nMessageBoard");

                entity.ToTable("ActivitiesMessageBoard");

                entity.Property(e => e.ActivitiesMessageId).HasColumnName("ActivitiesMessageID");

                entity.Property(e => e.ActivitiesCreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ActivitiesMessageState)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ActivitiesMessageBoards)
                    .HasForeignKey(d => d.ActivityId)
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
                entity.Property(e => e.AdministratorId).HasColumnName("AdministratorID");

                entity.Property(e => e.AdministratorName).HasMaxLength(50);

                entity.Property(e => e.AdministratorStatusId).HasColumnName("AdministratorStatusID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.AdministratorStatus)
                    .WithMany(p => p.Administrators)
                    .HasForeignKey(d => d.AdministratorStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Administrators_AdministratorStatusCategory");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Administrators)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Adminstrators_Users");
            });

            modelBuilder.Entity<AdministratorStatusCategory>(entity =>
            {
                entity.HasKey(e => e.AdministratorStatusId)
                    .HasName("PK__Administ__7EDF6BB32262A51C");

                entity.ToTable("AdministratorStatusCategory");

                entity.Property(e => e.AdministratorStatusId).HasColumnName("AdministratorStatusID");

                entity.Property(e => e.StatusName).HasMaxLength(10);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Age).HasComputedColumnSql("(datediff(year,[Birthday],getdate()))", false);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CellPhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.IdentityNumber).HasMaxLength(50);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.Property(e => e.MemberStatusId).HasColumnName("MemberStatusID");

                entity.Property(e => e.NickName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.MemberStatus)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.MemberStatusId)
                    .HasConstraintName("FK_Members_MemberStatusCategory");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Members_Users");
            });

            modelBuilder.Entity<MemberStatusCategory>(entity =>
            {
                entity.HasKey(e => e.MemberStatusId)
                    .HasName("PK__MemberSt__53C4FDDCB6744793");

                entity.ToTable("MemberStatusCategory");

                entity.Property(e => e.MemberStatusId).HasColumnName("MemberStatusID");

                entity.Property(e => e.StatusName).HasMaxLength(20);
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
                entity.HasKey(e => e.OrderCategoryId)
                    .HasName("PK_OrderStatus\nCategories");

                entity.Property(e => e.OrderCategoryId).HasColumnName("OrderCategoryID");

                entity.Property(e => e.OrderCategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentStatusCategory>(entity =>
            {
                entity.HasKey(e => e.PaymentCategoryId)
                    .HasName("PK_PaymentStatus\nCategories");

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

                entity.Property(e => e.ProductStatusId).HasColumnName("ProductStatusID");

                entity.Property(e => e.ProductUnitPrice).HasColumnType("money");

                entity.Property(e => e.QuantityPerUnit).HasMaxLength(50);

                entity.Property(e => e.RemovedTime).HasColumnType("datetime");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_ProductCategories");

                entity.HasOne(d => d.ProductStatusNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductStatusId)
                    .HasConstraintName("FK_Products_ProductStatusCategoy");

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

            modelBuilder.Entity<ProductStatusCategoy>(entity =>
            {
                entity.HasKey(e => e.ProductStatusId)
                    .HasName("PK__ProductS__208205AB13BE0CE7");

                entity.ToTable("ProductStatusCategoy");

                entity.Property(e => e.ProductStatusId).HasColumnName("ProductStatusID");

                entity.Property(e => e.StatusName).HasMaxLength(20);
            });

            modelBuilder.Entity<ProductsMessageBoard>(entity =>
            {
                entity.HasKey(e => e.MessageBoardId)
                    .HasName("PK_Products\nMessageBoard");

                entity.ToTable("ProductsMessageBoard");

                entity.Property(e => e.MessageBoardId).HasColumnName("MessageBoardID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MessageCreatedTime).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductMessageContent).HasMaxLength(50);

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
                entity.HasKey(e => e.ProductPictureId)
                    .HasName("PK_Products\nPictureInfo");

                entity.ToTable("ProductsPictureInfo");

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
                entity.HasKey(e => e.ShipCategoryId)
                    .HasName("PK_ShipStatus\nCategories");

                entity.Property(e => e.ShipCategoryId).HasColumnName("ShipCategoryID");

                entity.Property(e => e.ShipCategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<Sightseeing>(entity =>
            {
                entity.ToTable("Sightseeing");

                entity.Property(e => e.SightseeingId).HasColumnName("SightseeingID");

                entity.Property(e => e.AdministratorId).HasColumnName("AdministratorID");

                entity.Property(e => e.SightseeingAddress).HasMaxLength(50);

                entity.Property(e => e.SightseeingCategoryId).HasColumnName("SightseeingCategoryID");

                entity.Property(e => e.SightseeingCity).HasMaxLength(50);

                entity.Property(e => e.SightseeingCountry).HasMaxLength(50);

                entity.Property(e => e.SightseeingName).HasMaxLength(50);

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Sightseeings)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sightseeing_Administrators");

                entity.HasOne(d => d.SightseeingCategory)
                    .WithMany(p => p.Sightseeings)
                    .HasForeignKey(d => d.SightseeingCategoryId)
                    .HasConstraintName("FK_Sightseeing_SightseeingCategories");
            });

            modelBuilder.Entity<SightseeingCategory>(entity =>
            {
                entity.Property(e => e.SightseeingCategoryId).HasColumnName("SightseeingCategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(20);
            });

            modelBuilder.Entity<SightseeingMessageBoard>(entity =>
            {
                entity.HasKey(e => e.MessageBoardId)
                    .HasName("PK_Sightseeing\nMessageBoard");

                entity.ToTable("SightseeingMessageBoard");

                entity.Property(e => e.MessageBoardId).HasColumnName("MessageBoardID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MessageCreatedTime).HasColumnType("datetime");

                entity.Property(e => e.SightseeingId).HasColumnName("SightseeingID");

                entity.Property(e => e.SightseeingStatusId).HasColumnName("SightseeingStatusID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.SightseeingMessageBoards)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sightseeing\nMessageBoard_Members");

                entity.HasOne(d => d.Sightseeing)
                    .WithMany(p => p.SightseeingMessageBoards)
                    .HasForeignKey(d => d.SightseeingId)
                    .HasConstraintName("FK_Sightseeing\nMessageBoard_Sightseeing");

                entity.HasOne(d => d.SightseeingStatus)
                    .WithMany(p => p.SightseeingMessageBoards)
                    .HasForeignKey(d => d.SightseeingStatusId)
                    .HasConstraintName("FK_Sightseeing\nMessageBoard_SightseeingStatusCategory");
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

            modelBuilder.Entity<SightseeingStatusCategory>(entity =>
            {
                entity.HasKey(e => e.SightseeingStatusId)
                    .HasName("PK__Sightsee__A82B64CABB66CBB3");

                entity.ToTable("SightseeingStatusCategory");

                entity.Property(e => e.SightseeingStatusId).HasColumnName("SightseeingStatusID");

                entity.Property(e => e.StatusName).HasMaxLength(20);
            });

            modelBuilder.Entity<SocialActivity>(entity =>
            {
                entity.HasKey(e => e.ActivityId);

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

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

                entity.Property(e => e.SupplierAddress).HasMaxLength(50);

                entity.Property(e => e.SupplierCity).HasMaxLength(20);

                entity.Property(e => e.SupplierCountry).HasMaxLength(20);

                entity.Property(e => e.SupplierPhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierPostalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierStatusId).HasColumnName("SupplierStatusID");

                entity.Property(e => e.TaxId).HasColumnName("TaxID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.SupplierStatus)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.SupplierStatusId)
                    .HasConstraintName("FK_Suppliers_SupplierStatusCategoy");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Suppliers_Users");
            });

            modelBuilder.Entity<SupplierStatusCategoy>(entity =>
            {
                entity.HasKey(e => e.SupplierStatusId)
                    .HasName("PK__Supplier__6F9CA1BBBB698C79");

                entity.ToTable("SupplierStatusCategoy");

                entity.Property(e => e.SupplierStatusId).HasColumnName("SupplierStatusID");

                entity.Property(e => e.StatusName).HasMaxLength(20);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

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

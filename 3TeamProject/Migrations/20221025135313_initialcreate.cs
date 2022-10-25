using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3TeamProject.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdministratorStatusCategory",
                columns: table => new
                {
                    AdministratorStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Administ__7EDF6BB32262A51C", x => x.AdministratorStatusID);
                });

            migrationBuilder.CreateTable(
                name: "MemberStatusCategory",
                columns: table => new
                {
                    MemberStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MemberSt__53C4FDDCB6744793", x => x.MemberStatusID);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusCategories",
                columns: table => new
                {
                    OrderCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus\nCategories", x => x.OrderCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatusCategories",
                columns: table => new
                {
                    PaymentCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatus\nCategories", x => x.PaymentCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "ProductStatusCategoy",
                columns: table => new
                {
                    ProductStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductS__208205AB13BE0CE7", x => x.ProductStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "ShipStatusCategories",
                columns: table => new
                {
                    ShipCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipStatus\nCategories", x => x.ShipCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "SightseeingCategories",
                columns: table => new
                {
                    SightseeingCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SightseeingCategories", x => x.SightseeingCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "SightseeingMessageStatusCategory",
                columns: table => new
                {
                    SightseeingStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sightsee__A82B64CABB66CBB3", x => x.SightseeingStatusID);
                });

            migrationBuilder.CreateTable(
                name: "SupplierStatusCategoy",
                columns: table => new
                {
                    SupplierStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__6F9CA1BBBB698C79", x => x.SupplierStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(128)", maxLength: 128, nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(128)", maxLength: 128, nullable: false),
                    Roles = table.Column<int>(type: "int", nullable: true),
                    VerficationToken = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    PasswordResetToken = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    VerfiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles1",
                        column: x => x.Roles,
                        principalTable: "Roles",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    AdministratorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdministratorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    AdministratorStatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.AdministratorID);
                    table.ForeignKey(
                        name: "FK_Administrators_AdministratorStatusCategory",
                        column: x => x.AdministratorStatusID,
                        principalTable: "AdministratorStatusCategory",
                        principalColumn: "AdministratorStatusID");
                    table.ForeignKey(
                        name: "FK_Adminstrators_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CellPhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(datediff(year,[Birthday],getdate()))", stored: false),
                    MemberStatusID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_Members_MemberStatusCategory",
                        column: x => x.MemberStatusID,
                        principalTable: "MemberStatusCategory",
                        principalColumn: "MemberStatusID");
                    table.ForeignKey(
                        name: "FK_Members_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SuppliersID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxID = table.Column<int>(type: "int", nullable: false),
                    Fax = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CellPhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    SupplierPhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    SupplierPostalCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    SupplierCountry = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SupplierCity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SupplierAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    SupplierStatusID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SuppliersID);
                    table.ForeignKey(
                        name: "FK_Suppliers_SupplierStatusCategoy",
                        column: x => x.SupplierStatusID,
                        principalTable: "SupplierStatusCategoy",
                        principalColumn: "SupplierStatusID");
                    table.ForeignKey(
                        name: "FK_Suppliers_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sightseeing",
                columns: table => new
                {
                    SightseeingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SightseeingName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SightseeingCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SightseeingCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SightseeingAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SightseeingScore = table.Column<double>(type: "float", nullable: true),
                    AdministratorID = table.Column<int>(type: "int", nullable: false),
                    SightseeingIntroduce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SightseeingHomePage = table.Column<int>(type: "int", nullable: true),
                    SightseeingCategoryID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sightseeing", x => x.SightseeingID);
                    table.ForeignKey(
                        name: "FK_Sightseeing_Administrators",
                        column: x => x.AdministratorID,
                        principalTable: "Administrators",
                        principalColumn: "AdministratorID");
                    table.ForeignKey(
                        name: "FK_Sightseeing_SightseeingCategories",
                        column: x => x.SightseeingCategoryID,
                        principalTable: "SightseeingCategories",
                        principalColumn: "SightseeingCategoryID");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    AdministratorID = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ShipDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    ShipStatus = table.Column<int>(type: "int", nullable: false),
                    ShipPostalCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ShipCountry = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ShipCity = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ShipAddress = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Administrators",
                        column: x => x.AdministratorID,
                        principalTable: "Administrators",
                        principalColumn: "AdministratorID");
                    table.ForeignKey(
                        name: "FK_Orders_Members",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatus\nCategories",
                        column: x => x.OrderStatus,
                        principalTable: "OrderStatusCategories",
                        principalColumn: "OrderCategoryID");
                    table.ForeignKey(
                        name: "FK_Orders_PaymentStatus\nCategories",
                        column: x => x.PaymentStatus,
                        principalTable: "PaymentStatusCategories",
                        principalColumn: "PaymentCategoryID");
                    table.ForeignKey(
                        name: "FK_Orders_ShipStatus\nCategories",
                        column: x => x.ShipStatus,
                        principalTable: "ShipStatusCategories",
                        principalColumn: "ShipCategoryID");
                });

            migrationBuilder.CreateTable(
                name: "SocialActivities",
                columns: table => new
                {
                    ActivityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    ActivitiesName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActivitiesContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivitiesAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LimitCount = table.Column<int>(type: "int", nullable: true),
                    JoinCount = table.Column<int>(type: "int", nullable: false),
                    ActitiesStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ActitiesFinishDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialActivities", x => x.ActivityID);
                    table.ForeignKey(
                        name: "FK_SocialActivities_Members",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryID = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuantityPerUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductUnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    UnitStock = table.Column<int>(type: "int", nullable: true),
                    UniOnOrder = table.Column<int>(type: "int", nullable: true),
                    ProductRecommendation = table.Column<int>(type: "int", nullable: true),
                    AddedTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    RemovedTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProductIntroduce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductStatusID = table.Column<int>(type: "int", nullable: true),
                    ProductHomePage = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories",
                        column: x => x.ProductCategoryID,
                        principalTable: "ProductCategories",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_Products_ProductStatusCategoy",
                        column: x => x.ProductStatusID,
                        principalTable: "ProductStatusCategoy",
                        principalColumn: "ProductStatusID");
                    table.ForeignKey(
                        name: "FK_Products_Suppliers",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SuppliersID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SightseeingMessageBoard",
                columns: table => new
                {
                    MessageBoardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SightseeingID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    SightseeingMessageContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageCreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    SightseeingStatusID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sightseeing\nMessageBoard", x => x.MessageBoardID);
                    table.ForeignKey(
                        name: "FK_Sightseeing\nMessageBoard_Sightseeing",
                        column: x => x.SightseeingID,
                        principalTable: "Sightseeing",
                        principalColumn: "SightseeingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sightseeing\nMessageBoard_SightseeingStatusCategory",
                        column: x => x.SightseeingStatusID,
                        principalTable: "SightseeingMessageStatusCategory",
                        principalColumn: "SightseeingStatusID");
                    table.ForeignKey(
                        name: "FK_SightseeingMessageBoard_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "SightseeingPictureInfo",
                columns: table => new
                {
                    SightseeingPictureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SightseeingID = table.Column<int>(type: "int", nullable: false),
                    SightseeingPicturePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SightseeingPictureName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SightseeingPictureInfo", x => x.SightseeingPictureID);
                    table.ForeignKey(
                        name: "FK_SightseeingPictureInfo_Sightseeing",
                        column: x => x.SightseeingID,
                        principalTable: "Sightseeing",
                        principalColumn: "SightseeingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivitiesMessageBoard",
                columns: table => new
                {
                    ActivitiesMessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityID = table.Column<int>(type: "int", nullable: false),
                    ActivitiesMessageContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivitiesCreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ActivitiesMessageState = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities\nMessageBoard", x => x.ActivitiesMessageID);
                    table.ForeignKey(
                        name: "FK_Activities\nMessageBoard_SocialActivities",
                        column: x => x.ActivityID,
                        principalTable: "SocialActivities",
                        principalColumn: "ActivityID");
                    table.ForeignKey(
                        name: "FK_ActivitiesMessageBoard_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => new { x.OrderID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "ProductsMessageBoard",
                columns: table => new
                {
                    MessageBoardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    ProductMessageContent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MessageCreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductMessageStatus = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products\nMessageBoard", x => x.MessageBoardID);
                    table.ForeignKey(
                        name: "FK_Products\nMessageBoard_Members",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_Products\nMessageBoard_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "ProductsPictureInfo",
                columns: table => new
                {
                    ProductPictureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ProductPicturePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductPictureName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products\nPictureInfo", x => x.ProductPictureID);
                    table.ForeignKey(
                        name: "FK_Products\nPictureInfo_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivitiesMessageBoard_ActivityID",
                table: "ActivitiesMessageBoard",
                column: "ActivityID");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitiesMessageBoard_UserID",
                table: "ActivitiesMessageBoard",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_AdministratorStatusID",
                table: "Administrators",
                column: "AdministratorStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_UserID",
                table: "Administrators",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberStatusID",
                table: "Members",
                column: "MemberStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserID",
                table: "Members",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AdministratorID",
                table: "Orders",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MemberID",
                table: "Orders",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatus",
                table: "Orders",
                column: "OrderStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentStatus",
                table: "Orders",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipStatus",
                table: "Orders",
                column: "ShipStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryID",
                table: "Products",
                column: "ProductCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductStatusID",
                table: "Products",
                column: "ProductStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierID",
                table: "Products",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsMessageBoard_MemberID",
                table: "ProductsMessageBoard",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsMessageBoard_ProductID",
                table: "ProductsMessageBoard",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsPictureInfo_ProductID",
                table: "ProductsPictureInfo",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Sightseeing_AdministratorID",
                table: "Sightseeing",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_Sightseeing_SightseeingCategoryID",
                table: "Sightseeing",
                column: "SightseeingCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_SightseeingMessageBoard_SightseeingID",
                table: "SightseeingMessageBoard",
                column: "SightseeingID");

            migrationBuilder.CreateIndex(
                name: "IX_SightseeingMessageBoard_SightseeingStatusID",
                table: "SightseeingMessageBoard",
                column: "SightseeingStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_SightseeingMessageBoard_UserID",
                table: "SightseeingMessageBoard",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SightseeingPictureInfo_SightseeingID",
                table: "SightseeingPictureInfo",
                column: "SightseeingID");

            migrationBuilder.CreateIndex(
                name: "IX_SocialActivities_MemberID",
                table: "SocialActivities",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierStatusID",
                table: "Suppliers",
                column: "SupplierStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_UserID",
                table: "Suppliers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Roles",
                table: "Users",
                column: "Roles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivitiesMessageBoard");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductsMessageBoard");

            migrationBuilder.DropTable(
                name: "ProductsPictureInfo");

            migrationBuilder.DropTable(
                name: "SightseeingMessageBoard");

            migrationBuilder.DropTable(
                name: "SightseeingPictureInfo");

            migrationBuilder.DropTable(
                name: "SocialActivities");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SightseeingMessageStatusCategory");

            migrationBuilder.DropTable(
                name: "Sightseeing");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "OrderStatusCategories");

            migrationBuilder.DropTable(
                name: "PaymentStatusCategories");

            migrationBuilder.DropTable(
                name: "ShipStatusCategories");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductStatusCategoy");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "SightseeingCategories");

            migrationBuilder.DropTable(
                name: "MemberStatusCategory");

            migrationBuilder.DropTable(
                name: "SupplierStatusCategoy");

            migrationBuilder.DropTable(
                name: "AdministratorStatusCategory");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Context.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdateBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimePreparing = table.Column<int>(type: "int", nullable: false),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableNumber = table.Column<int>(type: "int", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAgeBased = table.Column<bool>(type: "bit", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discounts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MenuProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinTime = table.Column<int>(type: "int", nullable: false),
                    MaxTime = table.Column<int>(type: "int", nullable: false),
                    DayStock = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdateBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MenuProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_MenuProducts_MenuProductId",
                        column: x => x.MenuProductId,
                        principalTable: "MenuProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MenuProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_MenuProducts_MenuProductId",
                        column: x => x.MenuProductId,
                        principalTable: "MenuProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MenuProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuProducts_MenuProductId",
                        column: x => x.MenuProductId,
                        principalTable: "MenuProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Birthday", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "ImageURL", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { "1", 0, new DateTime(1999, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "803717a9-4924-4252-adee-e9194ff3521d", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "medo03459@gmail.com", true, null, true, false, null, "MEDO03459@GMAIL.COM", "MEDO03459", "AQAAAAIAAYagAAAAEGujmCBFWVkseMSmwkHJzelfLQqoDKV+HJz237coxCt+D2110MiU+PozkkFpJ39rbA==", "+201123002663", false, "a25304c2-a47a-44b5-99f5-a8e0b0440f93", false, null, "medo03459" },
                    { "2", 0, new DateTime(1999, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "8aed222c-908e-4219-a435-f6c6038b8298", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "4dm1n@gmail.com", true, null, true, false, null, "4DM1N@GMAIL.COM", "4DM1N", "AQAAAAIAAYagAAAAEOJRs4ccVxOzLJOvdBlxF2Ivs7dO0Tcxsmp3URw3mOwc25pH64//Zm8TgpwTRn6pSA==", "+201123002663", false, "f8170840-6975-4a6e-ab4f-49bf7d86bafb", false, null, "4dm1n" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "IsDeletedBy", "IsUpdateBy", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, "Salads", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, "Soups", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, "Burgers", null },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, "Cakes & Desserts", null },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, "Beverages", null },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, "Seafood", null },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, "Grilled & BBQ", null },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, "Pizza", null },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, "Pies & Pastries", null }
                });

            migrationBuilder.InsertData(
                table: "MenuProducts",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "DayStock", "Description", "ImageUrl", "IsDeleted", "IsDeletedBy", "IsUpdateBy", "MaxTime", "MinTime", "Name", "Price", "Quantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Fresh mixed greens with cherry tomatoes, cucumbers, and house dressing", "/images/a1.png", false, null, null, 10, 5, "Fresh Garden Salad", 45m, 50, null },
                    { 2, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Colorful assorted fresh fruits with honey drizzle", "/images/a2.png", false, null, null, 8, 5, "Fruit Paradise Bowl", 55m, 40, null },
                    { 3, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Rich and creamy sweet corn soup", "/images/a3.png", false, null, null, 12, 8, "Creamy Corn Soup", 35m, 60, null },
                    { 4, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Traditional tomato soup with fresh basil", "/images/a4.png", false, null, null, 12, 8, "Classic Tomato Soup", 30m, 55, null },
                    { 5, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Velvety mushroom soup with herbs", "/images/a5.png", false, null, null, 15, 10, "Creamy Mushroom Soup", 40m, 50, null },
                    { 6, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Rich seafood soup with prawns and fish", "/images/a6.png", false, null, null, 18, 12, "Seafood Chowder", 65m, 35, null },
                    { 7, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Light and fluffy egg drop soup", "/images/a7.png", false, null, null, 10, 8, "Golden Egg Drop Soup", 28m, 60, null },
                    { 8, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "Crispy golden French fries", "/images/a8.png", false, null, null, 12, 8, "Golden French Fries", 25m, 100, null },
                    { 9, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 200, "Classic tomato ketchup", "/images/a9.png", false, null, null, 2, 1, "Ketchup Dip", 8m, 200, null },
                    { 10, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "Juicy beef patty with cheese and fresh vegetables", "/images/b1.png", false, null, null, 20, 15, "Classic Beef Burger", 55m, 80, null },
                    { 11, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Two beef patties with double cheese", "/images/b2.png", false, null, null, 25, 18, "Double Decker Burger", 75m, 60, null },
                    { 12, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Three layers of beef patties with special sauce", "/images/b3.png", false, null, null, 28, 20, "Triple Stack Burger", 95m, 45, null },
                    { 13, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Huge burger with multiple patties and toppings", "/images/b4.png", false, null, null, 30, 25, "Mega Monster Burger", 110m, 30, null },
                    { 14, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Burger combo with fries", "/images/b5.png", false, null, null, 22, 18, "Deluxe Combo Burger", 65m, 70, null },
                    { 15, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, "Crispy fried chicken with fresh lettuce", "/images/b6.png", false, null, null, 20, 15, "Crispy Chicken Burger", 50m, 75, null },
                    { 16, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Beef burger with crispy bacon and BBQ sauce", "/images/b7.png", false, null, null, 23, 18, "BBQ Bacon Burger", 70m, 55, null },
                    { 17, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Premium beef with caramelized onions and special sauce", "/images/b8.png", false, null, null, 25, 20, "Premium Gourmet Burger", 85m, 40, null },
                    { 18, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Spicy burger with jalapeños and pepper jack cheese", "/images/b9.png", false, null, null, 22, 16, "Spicy Jalapeño Burger", 60m, 65, null },
                    { 19, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "Soft vanilla cupcake with cream frosting", "/images/c1.png", false, null, null, 8, 5, "Vanilla Cupcake", 20m, 100, null },
                    { 20, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 95, "Rich chocolate cupcake with chocolate frosting", "/images/c2.png", false, null, null, 8, 5, "Chocolate Cupcake", 22m, 95, null },
                    { 21, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Tangy passion fruit tart with fresh cream", "/images/c3.png", false, null, null, 10, 8, "Passion Fruit Tart", 35m, 50, null },
                    { 22, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, "Classic Black Forest cake with cherries and whipped cream", "/images/c4.png", false, null, null, 15, 10, "Black Forest Cake", 120m, 20, null },
                    { 23, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Creamy cheesecake topped with fresh strawberries", "/images/c5.png", false, null, null, 12, 8, "Strawberry Cheesecake", 65m, 40, null },
                    { 24, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Chocolate cake with Oreo cream filling", "/images/c6.png", false, null, null, 12, 8, "Cookies & Cream Cake", 55m, 45, null },
                    { 25, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Classic red velvet cake with cream cheese frosting", "/images/c7.png", false, null, null, 10, 6, "Red Velvet Cake Slice", 45m, 60, null },
                    { 26, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Tangy lemon filling with fluffy meringue topping", "/images/c8.png", false, null, null, 10, 8, "Lemon Meringue Pie", 40m, 35, null },
                    { 27, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "Chocolate and vanilla ice cream with toppings", "/images/c9.png", false, null, null, 8, 5, "Ice Cream Sundae", 38m, 80, null },
                    { 28, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Healthy green smoothie with spinach and fruits", "/images/d2.png", false, null, null, 8, 5, "Green Detox Smoothie", 32m, 60, null },
                    { 29, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Refreshing mint mojito with lime", "/images/d3.png", false, null, null, 8, 5, "Mint Mojito", 28m, 70, null },
                    { 30, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "Freshly squeezed orange juice", "/images/d4.png", false, null, null, 7, 5, "Fresh Orange Juice", 25m, 80, null },
                    { 31, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Creamy strawberry milkshake", "/images/d5.png", false, null, null, 10, 6, "Strawberry Milkshake", 35m, 65, null },
                    { 32, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, "Fresh watermelon juice", "/images/d6.png", false, null, null, 8, 5, "Watermelon Juice", 22m, 75, null },
                    { 33, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, "Classic fresh lemonade", "/images/d7.png", false, null, null, 7, 5, "Lemonade", 20m, 90, null },
                    { 34, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Thick strawberry smoothie", "/images/d8.png", false, null, null, 9, 6, "Strawberry Smoothie", 30m, 70, null },
                    { 35, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Tropical mango smoothie", "/images/d9.png", false, null, null, 9, 6, "Mango Smoothie", 32m, 65, null },
                    { 36, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "Traditional hot tea", "/images/d10.png", false, null, null, 8, 5, "Hot Tea", 15m, 100, null },
                    { 37, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 85, "Rich Italian cappuccino", "/images/d11.png", false, null, null, 10, 6, "Cappuccino", 28m, 85, null },
                    { 38, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, "Artistic latte with milk foam", "/images/d12.png", false, null, null, 12, 7, "Latte Art Coffee", 30m, 75, null },
                    { 39, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, "Strong Italian espresso", "/images/d13.png", false, null, null, 8, 5, "Espresso", 25m, 90, null },
                    { 40, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Fresh shrimp with cocktail sauce", "/images/f1.png", false, null, null, 20, 15, "Shrimp Platter", 95m, 40, null },
                    { 41, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Assorted fresh sushi rolls", "/images/f2.png", false, null, null, 25, 20, "Sushi Platter", 120m, 35, null },
                    { 42, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Perfectly grilled fish with lemon", "/images/f3.png", false, null, null, 25, 18, "Grilled Fish Fillet", 85m, 45, null },
                    { 43, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Premium grilled salmon steak", "/images/f4.png", false, null, null, 28, 20, "Grilled Salmon", 110m, 30, null },
                    { 44, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Crispy fried calamari rings", "/images/f5.png", false, null, null, 18, 12, "Calamari Rings", 65m, 55, null },
                    { 45, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Classic British fish and chips", "/images/f6.png", false, null, null, 22, 15, "Fish & Chips", 70m, 50, null },
                    { 46, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, "Crispy breaded jumbo shrimp", "/images/f7.png", false, null, null, 20, 15, "Breaded Shrimp", 75m, 48, null },
                    { 47, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, "Spanish seafood rice with mixed seafood", "/images/f8.png", false, null, null, 35, 25, "Seafood Paella", 130m, 25, null },
                    { 48, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "Grilled whole lobster with butter sauce", "/images/f9.png", false, null, null, 40, 30, "Lobster Special", 180m, 15, null },
                    { 49, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Hot and spicy chicken wings", "/images/g1.png", false, null, null, 20, 15, "Spicy Wings", 55m, 70, null },
                    { 50, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Chicken wings with BBQ glaze", "/images/g2.png", false, null, null, 22, 15, "BBQ Chicken Wings", 58m, 65, null },
                    { 51, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Assorted grilled meats with vegetables", "/images/g3.png", false, null, null, 35, 25, "Mixed Grill Platter", 140m, 35, null },
                    { 52, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Premium beef steak grilled to perfection", "/images/g4.png", false, null, null, 30, 22, "Grilled Steak", 150m, 30, null },
                    { 53, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Grilled chicken kebabs with spices", "/images/g5.png", false, null, null, 25, 18, "Chicken Kebab Skewers", 75m, 55, null },
                    { 54, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Tender BBQ pork ribs", "/images/g6.png", false, null, null, 35, 25, "BBQ Ribs", 95m, 40, null },
                    { 55, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, "Juicy grilled lamb chops with herbs", "/images/g7.png", false, null, null, 30, 22, "Grilled Lamb Chops", 135m, 28, null },
                    { 56, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 32, "Assorted meat kebabs with grilled vegetables", "/images/g8.png", false, null, null, 28, 22, "Mixed Kebab Platter", 120m, 32, null },
                    { 57, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Grilled chicken drumsticks with spices", "/images/g9.png", false, null, null, 25, 18, "Chicken Drumsticks", 65m, 60, null },
                    { 58, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Sticky BBQ chicken wings with sauce", "/images/g111.png", false, null, null, 25, 20, "BBQ Wings Special", 85m, 45, null },
                    { 59, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Classic pizza with tomato, mozzarella, and basil", "/images/p1.png", false, null, null, 22, 15, "Margherita Pizza", 65m, 50, null },
                    { 60, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Classic pepperoni with mozzarella cheese", "/images/p2.png", false, null, null, 22, 15, "Pepperoni Pizza", 75m, 55, null },
                    { 61, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Loaded with pepperoni, sausage, peppers, and more", "/images/p3.png", false, null, null, 25, 18, "Supreme Pizza", 95m, 45, null },
                    { 62, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, "Fresh vegetables with mozzarella and tomato sauce", "/images/p4.png", false, null, null, 22, 15, "Vegetarian Pizza", 70m, 48, null },
                    { 63, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 42, "Four cheese blend pizza", "/images/p5.png", false, null, null, 23, 16, "Cheese Lovers Pizza", 80m, 42, null },
                    { 64, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Grilled chicken with BBQ sauce and onions", "/images/p6.png", false, null, null, 25, 18, "BBQ Chicken Pizza", 85m, 40, null },
                    { 65, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 46, "Ham and pineapple with mozzarella", "/images/p7.png", false, null, null, 22, 15, "Hawaiian Pizza", 75m, 46, null },
                    { 66, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 38, "Loaded with various meats and cheese", "/images/p8.png", false, null, null, 26, 18, "Meat Feast Pizza", 98m, 38, null },
                    { 67, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Classic American apple pie with cinnamon", "/images/s1.png", false, null, null, 15, 10, "Apple Pie", 55m, 45, null },
                    { 68, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Sweet cherry pie with lattice crust", "/images/s2.png", false, null, null, 15, 10, "Cherry Pie", 58m, 40, null },
                    { 69, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "French crepes with sweet filling", "/images/s3.png", false, null, null, 12, 8, "Crepes", 42m, 60, null },
                    { 70, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Belgian waffle topped with fresh strawberries", "/images/s4.png", false, null, null, 15, 10, "Strawberry Waffle", 48m, 55, null },
                    { 71, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "Flaky French butter croissants", "/images/s5.png", false, null, null, 8, 5, "Butter Croissants", 25m, 80, null },
                    { 72, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Sweet cinnamon roll with cream cheese frosting", "/images/s6.png", false, null, null, 12, 8, "Cinnamon Roll", 32m, 70, null },
                    { 73, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Assorted sweet pastries and cookies", "/images/s7.png", false, null, null, 12, 10, "Mixed Pastry Box", 65m, 35, null },
                    { 74, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Assorted Danish pastries with fruit filling", "/images/s8.png", false, null, null, 10, 8, "Danish Pastries", 38m, 65, null },
                    { 75, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Fresh breakfast sandwich with egg and cheese", "/images/s9.png", false, null, null, 15, 10, "Breakfast Sandwich", 45m, 60, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_MenuProductId",
                table: "Carts",
                column: "MenuProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId_MenuProductId",
                table: "Carts",
                columns: new[] { "UserId", "MenuProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_CategoryId",
                table: "Discounts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_MenuProductId",
                table: "Favorites",
                column: "MenuProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_MenuProductId",
                table: "Favorites",
                columns: new[] { "UserId", "MenuProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuProducts_CategoryId",
                table: "MenuProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuProducts_Name",
                table: "MenuProducts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuProductId",
                table: "OrderItems",
                column: "MenuProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedAt",
                table: "Orders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UniqueOrderId",
                table: "Orders",
                column: "UniqueOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MenuProducts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

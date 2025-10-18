using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Context.Migrations
{
    /// <inheritdoc />
    public partial class intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimePreparing = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderCart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuProductId = table.Column<int>(type: "int", nullable: false),
                    Mount = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    UniqueOrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCart_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    OrderCartId = table.Column<int>(type: "int", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuProducts_OrderCart_OrderCartId",
                        column: x => x.OrderCartId,
                        principalTable: "OrderCart",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MenuProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MenuProductId1 = table.Column<int>(type: "int", nullable: true),
                    OrderCartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_MenuProducts_MenuProductId",
                        column: x => x.MenuProductId,
                        principalTable: "MenuProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_MenuProducts_MenuProductId1",
                        column: x => x.MenuProductId1,
                        principalTable: "MenuProducts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Carts_OrderCart_OrderCartId",
                        column: x => x.OrderCartId,
                        principalTable: "OrderCart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MenuProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_MenuProducts_MenuProductId",
                        column: x => x.MenuProductId,
                        principalTable: "MenuProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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
                table: "Users",
                columns: new[] { "Id", "Birthday", "CreatedAt", "Email", "ImageURL", "IsAdmin", "Name", "Password", "Phone", "UpdatedAt" },
                values: new object[] { 1, new DateTime(1999, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "medo03459@gmail.com", null, true, "Super Admin", "VNXLLTMtvbSFApPKrkVZzoi2UWPx6l1OSzrEnXct7RQ=", "+201123002663", null });

            migrationBuilder.InsertData(
                table: "MenuProducts",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "DayStock", "Description", "ImageUrl", "IsDeleted", "IsDeletedBy", "IsUpdateBy", "MaxTime", "MinTime", "Name", "OrderCartId", "Price", "Quantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Fresh mixed greens with cherry tomatoes, cucumbers, and house dressing", "/images/a1.png", false, null, null, 10, 5, "Fresh Garden Salad", null, 45m, 50, null },
                    { 2, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Colorful assorted fresh fruits with honey drizzle", "/images/a2.png", false, null, null, 8, 5, "Fruit Paradise Bowl", null, 55m, 40, null },
                    { 3, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Rich and creamy sweet corn soup", "/images/a3.png", false, null, null, 12, 8, "Creamy Corn Soup", null, 35m, 60, null },
                    { 4, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Traditional tomato soup with fresh basil", "/images/a4.png", false, null, null, 12, 8, "Classic Tomato Soup", null, 30m, 55, null },
                    { 5, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Velvety mushroom soup with herbs", "/images/a5.png", false, null, null, 15, 10, "Creamy Mushroom Soup", null, 40m, 50, null },
                    { 6, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Rich seafood soup with prawns and fish", "/images/a6.png", false, null, null, 18, 12, "Seafood Chowder", null, 65m, 35, null },
                    { 7, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Light and fluffy egg drop soup", "/images/a7.png", false, null, null, 10, 8, "Golden Egg Drop Soup", null, 28m, 60, null },
                    { 8, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "Crispy golden French fries", "/images/a8.png", false, null, null, 12, 8, "Golden French Fries", null, 25m, 100, null },
                    { 9, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 200, "Classic tomato ketchup", "/images/a9.png", false, null, null, 2, 1, "Ketchup Dip", null, 8m, 200, null },
                    { 10, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "Juicy beef patty with cheese and fresh vegetables", "/images/b1.png", false, null, null, 20, 15, "Classic Beef Burger", null, 55m, 80, null },
                    { 11, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Two beef patties with double cheese", "/images/b2.png", false, null, null, 25, 18, "Double Decker Burger", null, 75m, 60, null },
                    { 12, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Three layers of beef patties with special sauce", "/images/b3.png", false, null, null, 28, 20, "Triple Stack Burger", null, 95m, 45, null },
                    { 13, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Huge burger with multiple patties and toppings", "/images/b4.png", false, null, null, 30, 25, "Mega Monster Burger", null, 110m, 30, null },
                    { 14, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Burger combo with fries", "/images/b5.png", false, null, null, 22, 18, "Deluxe Combo Burger", null, 65m, 70, null },
                    { 15, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, "Crispy fried chicken with fresh lettuce", "/images/b6.png", false, null, null, 20, 15, "Crispy Chicken Burger", null, 50m, 75, null },
                    { 16, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Beef burger with crispy bacon and BBQ sauce", "/images/b7.png", false, null, null, 23, 18, "BBQ Bacon Burger", null, 70m, 55, null },
                    { 17, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Premium beef with caramelized onions and special sauce", "/images/b8.png", false, null, null, 25, 20, "Premium Gourmet Burger", null, 85m, 40, null },
                    { 18, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Spicy burger with jalapeños and pepper jack cheese", "/images/b9.png", false, null, null, 22, 16, "Spicy Jalapeño Burger", null, 60m, 65, null },
                    { 19, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "Soft vanilla cupcake with cream frosting", "/images/c1.png", false, null, null, 8, 5, "Vanilla Cupcake", null, 20m, 100, null },
                    { 20, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 95, "Rich chocolate cupcake with chocolate frosting", "/images/c2.png", false, null, null, 8, 5, "Chocolate Cupcake", null, 22m, 95, null },
                    { 21, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Tangy passion fruit tart with fresh cream", "/images/c3.png", false, null, null, 10, 8, "Passion Fruit Tart", null, 35m, 50, null },
                    { 22, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, "Classic Black Forest cake with cherries and whipped cream", "/images/c4.png", false, null, null, 15, 10, "Black Forest Cake", null, 120m, 20, null },
                    { 23, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Creamy cheesecake topped with fresh strawberries", "/images/c5.png", false, null, null, 12, 8, "Strawberry Cheesecake", null, 65m, 40, null },
                    { 24, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Chocolate cake with Oreo cream filling", "/images/c6.png", false, null, null, 12, 8, "Cookies & Cream Cake", null, 55m, 45, null },
                    { 25, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Classic red velvet cake with cream cheese frosting", "/images/c7.png", false, null, null, 10, 6, "Red Velvet Cake Slice", null, 45m, 60, null },
                    { 26, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Tangy lemon filling with fluffy meringue topping", "/images/c8.png", false, null, null, 10, 8, "Lemon Meringue Pie", null, 40m, 35, null },
                    { 27, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "Chocolate and vanilla ice cream with toppings", "/images/c9.png", false, null, null, 8, 5, "Ice Cream Sundae", null, 38m, 80, null },
                    { 28, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Healthy green smoothie with spinach and fruits", "/images/d2.png", false, null, null, 8, 5, "Green Detox Smoothie", null, 32m, 60, null },
                    { 29, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Refreshing mint mojito with lime", "/images/d3.png", false, null, null, 8, 5, "Mint Mojito", null, 28m, 70, null },
                    { 30, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "Freshly squeezed orange juice", "/images/d4.png", false, null, null, 7, 5, "Fresh Orange Juice", null, 25m, 80, null },
                    { 31, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Creamy strawberry milkshake", "/images/d5.png", false, null, null, 10, 6, "Strawberry Milkshake", null, 35m, 65, null },
                    { 32, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, "Fresh watermelon juice", "/images/d6.png", false, null, null, 8, 5, "Watermelon Juice", null, 22m, 75, null },
                    { 33, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, "Classic fresh lemonade", "/images/d7.png", false, null, null, 7, 5, "Lemonade", null, 20m, 90, null },
                    { 34, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Thick strawberry smoothie", "/images/d8.png", false, null, null, 9, 6, "Strawberry Smoothie", null, 30m, 70, null },
                    { 35, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Tropical mango smoothie", "/images/d9.png", false, null, null, 9, 6, "Mango Smoothie", null, 32m, 65, null },
                    { 36, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "Traditional hot tea", "/images/d10.png", false, null, null, 8, 5, "Hot Tea", null, 15m, 100, null },
                    { 37, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 85, "Rich Italian cappuccino", "/images/d11.png", false, null, null, 10, 6, "Cappuccino", null, 28m, 85, null },
                    { 38, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, "Artistic latte with milk foam", "/images/d12.png", false, null, null, 12, 7, "Latte Art Coffee", null, 30m, 75, null },
                    { 39, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, "Strong Italian espresso", "/images/d13.png", false, null, null, 8, 5, "Espresso", null, 25m, 90, null },
                    { 40, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Fresh shrimp with cocktail sauce", "/images/f1.png", false, null, null, 20, 15, "Shrimp Platter", null, 95m, 40, null },
                    { 41, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Assorted fresh sushi rolls", "/images/f2.png", false, null, null, 25, 20, "Sushi Platter", null, 120m, 35, null },
                    { 42, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Perfectly grilled fish with lemon", "/images/f3.png", false, null, null, 25, 18, "Grilled Fish Fillet", null, 85m, 45, null },
                    { 43, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Premium grilled salmon steak", "/images/f4.png", false, null, null, 28, 20, "Grilled Salmon", null, 110m, 30, null },
                    { 44, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Crispy fried calamari rings", "/images/f5.png", false, null, null, 18, 12, "Calamari Rings", null, 65m, 55, null },
                    { 45, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Classic British fish and chips", "/images/f6.png", false, null, null, 22, 15, "Fish & Chips", null, 70m, 50, null },
                    { 46, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, "Crispy breaded jumbo shrimp", "/images/f7.png", false, null, null, 20, 15, "Breaded Shrimp", null, 75m, 48, null },
                    { 47, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, "Spanish seafood rice with mixed seafood", "/images/f8.png", false, null, null, 35, 25, "Seafood Paella", null, 130m, 25, null },
                    { 48, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "Grilled whole lobster with butter sauce", "/images/f9.png", false, null, null, 40, 30, "Lobster Special", null, 180m, 15, null },
                    { 49, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Hot and spicy chicken wings", "/images/g1.png", false, null, null, 20, 15, "Spicy Wings", null, 55m, 70, null },
                    { 50, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Chicken wings with BBQ glaze", "/images/g2.png", false, null, null, 22, 15, "BBQ Chicken Wings", null, 58m, 65, null },
                    { 51, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Assorted grilled meats with vegetables", "/images/g3.png", false, null, null, 35, 25, "Mixed Grill Platter", null, 140m, 35, null },
                    { 52, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Premium beef steak grilled to perfection", "/images/g4.png", false, null, null, 30, 22, "Grilled Steak", null, 150m, 30, null },
                    { 53, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Grilled chicken kebabs with spices", "/images/g5.png", false, null, null, 25, 18, "Chicken Kebab Skewers", null, 75m, 55, null },
                    { 54, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Tender BBQ pork ribs", "/images/g6.png", false, null, null, 35, 25, "BBQ Ribs", null, 95m, 40, null },
                    { 55, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, "Juicy grilled lamb chops with herbs", "/images/g7.png", false, null, null, 30, 22, "Grilled Lamb Chops", null, 135m, 28, null },
                    { 56, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 32, "Assorted meat kebabs with grilled vegetables", "/images/g8.png", false, null, null, 28, 22, "Mixed Kebab Platter", null, 120m, 32, null },
                    { 57, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Grilled chicken drumsticks with spices", "/images/g9.png", false, null, null, 25, 18, "Chicken Drumsticks", null, 65m, 60, null },
                    { 58, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Sticky BBQ chicken wings with sauce", "/images/g111.png", false, null, null, 25, 20, "BBQ Wings Special", null, 85m, 45, null },
                    { 59, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Classic pizza with tomato, mozzarella, and basil", "/images/p1.png", false, null, null, 22, 15, "Margherita Pizza", null, 65m, 50, null },
                    { 60, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Classic pepperoni with mozzarella cheese", "/images/p2.png", false, null, null, 22, 15, "Pepperoni Pizza", null, 75m, 55, null },
                    { 61, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Loaded with pepperoni, sausage, peppers, and more", "/images/p3.png", false, null, null, 25, 18, "Supreme Pizza", null, 95m, 45, null },
                    { 62, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, "Fresh vegetables with mozzarella and tomato sauce", "/images/p4.png", false, null, null, 22, 15, "Vegetarian Pizza", null, 70m, 48, null },
                    { 63, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 42, "Four cheese blend pizza", "/images/p5.png", false, null, null, 23, 16, "Cheese Lovers Pizza", null, 80m, 42, null },
                    { 64, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Grilled chicken with BBQ sauce and onions", "/images/p6.png", false, null, null, 25, 18, "BBQ Chicken Pizza", null, 85m, 40, null },
                    { 65, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 46, "Ham and pineapple with mozzarella", "/images/p7.png", false, null, null, 22, 15, "Hawaiian Pizza", null, 75m, 46, null },
                    { 66, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 38, "Loaded with various meats and cheese", "/images/p8.png", false, null, null, 26, 18, "Meat Feast Pizza", null, 98m, 38, null },
                    { 67, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Classic American apple pie with cinnamon", "/images/s1.png", false, null, null, 15, 10, "Apple Pie", null, 55m, 45, null },
                    { 68, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Sweet cherry pie with lattice crust", "/images/s2.png", false, null, null, 15, 10, "Cherry Pie", null, 58m, 40, null },
                    { 69, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "French crepes with sweet filling", "/images/s3.png", false, null, null, 12, 8, "Crepes", null, 42m, 60, null },
                    { 70, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, "Belgian waffle topped with fresh strawberries", "/images/s4.png", false, null, null, 15, 10, "Strawberry Waffle", null, 48m, 55, null },
                    { 71, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "Flaky French butter croissants", "/images/s5.png", false, null, null, 8, 5, "Butter Croissants", null, 25m, 80, null },
                    { 72, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Sweet cinnamon roll with cream cheese frosting", "/images/s6.png", false, null, null, 12, 8, "Cinnamon Roll", null, 32m, 70, null },
                    { 73, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Assorted sweet pastries and cookies", "/images/s7.png", false, null, null, 12, 10, "Mixed Pastry Box", null, 65m, 35, null },
                    { 74, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, "Assorted Danish pastries with fruit filling", "/images/s8.png", false, null, null, 10, 8, "Danish Pastries", null, 38m, 65, null },
                    { 75, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Fresh breakfast sandwich with egg and cheese", "/images/s9.png", false, null, null, 15, 10, "Breakfast Sandwich", null, 45m, 60, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_MenuProductId",
                table: "Carts",
                column: "MenuProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_MenuProductId1",
                table: "Carts",
                column: "MenuProductId1",
                unique: true,
                filter: "[MenuProductId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_OrderCartId",
                table: "Carts",
                column: "OrderCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId_MenuProductId",
                table: "Carts",
                columns: new[] { "UserId", "MenuProductId" },
                unique: true);

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
                name: "IX_MenuProducts_OrderCartId",
                table: "MenuProducts",
                column: "OrderCartId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCart_OrderId",
                table: "OrderCart",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuProductId",
                table: "OrderItems",
                column: "MenuProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

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
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "MenuProducts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "OrderCart");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

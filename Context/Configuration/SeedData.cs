using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Context.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            var categories = new[]
            {
                new Category { Id = 1, Name = "Salads", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 2, Name = "Soups", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 3, Name = "Burgers", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 4, Name = "Cakes & Desserts", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 5, Name = "Beverages", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 6, Name = "Seafood", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 7, Name = "Grilled & BBQ", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 8, Name = "Pizza", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 9, Name = "Pies & Pastries", CreatedAt = new DateTime(2024, 1, 1) }
            };

            builder.HasData(categories);
        }
    }

        public class MenuProductConfiguration : IEntityTypeConfiguration<MenuProduct>
        {
            public void Configure(EntityTypeBuilder<MenuProduct> builder)
            {
                var baseDate = new DateTime(2024, 1, 1);
                var products = new List<MenuProduct>();
                int productId = 1;

                // SALADS
                AddProduct(products, ref productId, "Fresh Garden Salad", 1, 45, 50, "Fresh mixed greens with cherry tomatoes, cucumbers, and house dressing", "/images/a1.png", 5, 10, 50, baseDate);
                AddProduct(products, ref productId, "Fruit Paradise Bowl", 1, 55, 40, "Colorful assorted fresh fruits with honey drizzle", "/images/a2.png", 5, 8, 40, baseDate);

                // SOUPS
                AddProduct(products, ref productId, "Creamy Corn Soup", 2, 35, 60, "Rich and creamy sweet corn soup", "/images/a3.png", 8, 12, 60, baseDate);
                AddProduct(products, ref productId, "Classic Tomato Soup", 2, 30, 55, "Traditional tomato soup with fresh basil", "/images/a4.png", 8, 12, 55, baseDate);
                AddProduct(products, ref productId, "Creamy Mushroom Soup", 2, 40, 50, "Velvety mushroom soup with herbs", "/images/a5.png", 10, 15, 50, baseDate);
                AddProduct(products, ref productId, "Seafood Chowder", 2, 65, 35, "Rich seafood soup with prawns and fish", "/images/a6.png", 12, 18, 35, baseDate);
                AddProduct(products, ref productId, "Golden Egg Drop Soup", 2, 28, 60, "Light and fluffy egg drop soup", "/images/a7.png", 8, 10, 60, baseDate);

                // BURGERS & SIDES
                AddProduct(products, ref productId, "Golden French Fries", 3, 25, 100, "Crispy golden French fries", "/images/a8.png", 8, 12, 100, baseDate);
                AddProduct(products, ref productId, "Ketchup Dip", 3, 8, 200, "Classic tomato ketchup", "/images/a9.png", 1, 2, 200, baseDate);
                AddProduct(products, ref productId, "Classic Beef Burger", 3, 55, 80, "Juicy beef patty with cheese and fresh vegetables", "/images/b1.png", 15, 20, 80, baseDate);
                AddProduct(products, ref productId, "Double Decker Burger", 3, 75, 60, "Two beef patties with double cheese", "/images/b2.png", 18, 25, 60, baseDate);
                AddProduct(products, ref productId, "Triple Stack Burger", 3, 95, 45, "Three layers of beef patties with special sauce", "/images/b3.png", 20, 28, 45, baseDate);
                AddProduct(products, ref productId, "Mega Monster Burger", 3, 110, 30, "Huge burger with multiple patties and toppings", "/images/b4.png", 25, 30, 30, baseDate);
                AddProduct(products, ref productId, "Deluxe Combo Burger", 3, 65, 70, "Burger combo with fries", "/images/b5.png", 18, 22, 70, baseDate);
                AddProduct(products, ref productId, "Crispy Chicken Burger", 3, 50, 75, "Crispy fried chicken with fresh lettuce", "/images/b6.png", 15, 20, 75, baseDate);
                AddProduct(products, ref productId, "BBQ Bacon Burger", 3, 70, 55, "Beef burger with crispy bacon and BBQ sauce", "/images/b7.png", 18, 23, 55, baseDate);
                AddProduct(products, ref productId, "Premium Gourmet Burger", 3, 85, 40, "Premium beef with caramelized onions and special sauce", "/images/b8.png", 20, 25, 40, baseDate);
                AddProduct(products, ref productId, "Spicy Jalapeño Burger", 3, 60, 65, "Spicy burger with jalapeños and pepper jack cheese", "/images/b9.png", 16, 22, 65, baseDate);

                // CAKES & DESSERTS
                AddProduct(products, ref productId, "Vanilla Cupcake", 4, 20, 100, "Soft vanilla cupcake with cream frosting", "/images/c1.png", 5, 8, 100, baseDate);
                AddProduct(products, ref productId, "Chocolate Cupcake", 4, 22, 95, "Rich chocolate cupcake with chocolate frosting", "/images/c2.png", 5, 8, 95, baseDate);
                AddProduct(products, ref productId, "Passion Fruit Tart", 4, 35, 50, "Tangy passion fruit tart with fresh cream", "/images/c3.png", 8, 10, 50, baseDate);
                AddProduct(products, ref productId, "Black Forest Cake", 4, 120, 20, "Classic Black Forest cake with cherries and whipped cream", "/images/c4.png", 10, 15, 20, baseDate);
                AddProduct(products, ref productId, "Strawberry Cheesecake", 4, 65, 40, "Creamy cheesecake topped with fresh strawberries", "/images/c5.png", 8, 12, 40, baseDate);
                AddProduct(products, ref productId, "Cookies & Cream Cake", 4, 55, 45, "Chocolate cake with Oreo cream filling", "/images/c6.png", 8, 12, 45, baseDate);
                AddProduct(products, ref productId, "Red Velvet Cake Slice", 4, 45, 60, "Classic red velvet cake with cream cheese frosting", "/images/c7.png", 6, 10, 60, baseDate);
                AddProduct(products, ref productId, "Lemon Meringue Pie", 4, 40, 35, "Tangy lemon filling with fluffy meringue topping", "/images/c8.png", 8, 10, 35, baseDate);
                AddProduct(products, ref productId, "Ice Cream Sundae", 4, 38, 80, "Chocolate and vanilla ice cream with toppings", "/images/c9.png", 5, 8, 80, baseDate);

                // BEVERAGES
                AddProduct(products, ref productId, "Green Detox Smoothie", 5, 32, 60, "Healthy green smoothie with spinach and fruits", "/images/d2.png", 5, 8, 60, baseDate);
                AddProduct(products, ref productId, "Mint Mojito", 5, 28, 70, "Refreshing mint mojito with lime", "/images/d3.png", 5, 8, 70, baseDate);
                AddProduct(products, ref productId, "Fresh Orange Juice", 5, 25, 80, "Freshly squeezed orange juice", "/images/d4.png", 5, 7, 80, baseDate);
                AddProduct(products, ref productId, "Strawberry Milkshake", 5, 35, 65, "Creamy strawberry milkshake", "/images/d5.png", 6, 10, 65, baseDate);
                AddProduct(products, ref productId, "Watermelon Juice", 5, 22, 75, "Fresh watermelon juice", "/images/d6.png", 5, 8, 75, baseDate);
                AddProduct(products, ref productId, "Lemonade", 5, 20, 90, "Classic fresh lemonade", "/images/d7.png", 5, 7, 90, baseDate);
                AddProduct(products, ref productId, "Strawberry Smoothie", 5, 30, 70, "Thick strawberry smoothie", "/images/d8.png", 6, 9, 70, baseDate);
                AddProduct(products, ref productId, "Mango Smoothie", 5, 32, 65, "Tropical mango smoothie", "/images/d9.png", 6, 9, 65, baseDate);
                AddProduct(products, ref productId, "Hot Tea", 5, 15, 100, "Traditional hot tea", "/images/d10.png", 5, 8, 100, baseDate);
                AddProduct(products, ref productId, "Cappuccino", 5, 28, 85, "Rich Italian cappuccino", "/images/d11.png", 6, 10, 85, baseDate);
                AddProduct(products, ref productId, "Latte Art Coffee", 5, 30, 75, "Artistic latte with milk foam", "/images/d12.png", 7, 12, 75, baseDate);
                AddProduct(products, ref productId, "Espresso", 5, 25, 90, "Strong Italian espresso", "/images/d13.png", 5, 8, 90, baseDate);

                // SEAFOOD
                AddProduct(products, ref productId, "Shrimp Platter", 6, 95, 40, "Fresh shrimp with cocktail sauce", "/images/f1.png", 15, 20, 40, baseDate);
                AddProduct(products, ref productId, "Sushi Platter", 6, 120, 35, "Assorted fresh sushi rolls", "/images/f2.png", 20, 25, 35, baseDate);
                AddProduct(products, ref productId, "Grilled Fish Fillet", 6, 85, 45, "Perfectly grilled fish with lemon", "/images/f3.png", 18, 25, 45, baseDate);
                AddProduct(products, ref productId, "Grilled Salmon", 6, 110, 30, "Premium grilled salmon steak", "/images/f4.png", 20, 28, 30, baseDate);
                AddProduct(products, ref productId, "Calamari Rings", 6, 65, 55, "Crispy fried calamari rings", "/images/f5.png", 12, 18, 55, baseDate);
                AddProduct(products, ref productId, "Fish & Chips", 6, 70, 50, "Classic British fish and chips", "/images/f6.png", 15, 22, 50, baseDate);
                AddProduct(products, ref productId, "Breaded Shrimp", 6, 75, 48, "Crispy breaded jumbo shrimp", "/images/f7.png", 15, 20, 48, baseDate);
                AddProduct(products, ref productId, "Seafood Paella", 6, 130, 25, "Spanish seafood rice with mixed seafood", "/images/f8.png", 25, 35, 25, baseDate);
                AddProduct(products, ref productId, "Lobster Special", 6, 180, 15, "Grilled whole lobster with butter sauce", "/images/f9.png", 30, 40, 15, baseDate);

                // GRILLED & BBQ
                AddProduct(products, ref productId, "Spicy Wings", 7, 55, 70, "Hot and spicy chicken wings", "/images/g1.png", 15, 20, 70, baseDate);
                AddProduct(products, ref productId, "BBQ Chicken Wings", 7, 58, 65, "Chicken wings with BBQ glaze", "/images/g2.png", 15, 22, 65, baseDate);
                AddProduct(products, ref productId, "Mixed Grill Platter", 7, 140, 35, "Assorted grilled meats with vegetables", "/images/g3.png", 25, 35, 35, baseDate);
                AddProduct(products, ref productId, "Grilled Steak", 7, 150, 30, "Premium beef steak grilled to perfection", "/images/g4.png", 22, 30, 30, baseDate);
                AddProduct(products, ref productId, "Chicken Kebab Skewers", 7, 75, 55, "Grilled chicken kebabs with spices", "/images/g5.png", 18, 25, 55, baseDate);
                AddProduct(products, ref productId, "BBQ Ribs", 7, 95, 40, "Tender BBQ pork ribs", "/images/g6.png", 25, 35, 40, baseDate);
                AddProduct(products, ref productId, "Grilled Lamb Chops", 7, 135, 28, "Juicy grilled lamb chops with herbs", "/images/g7.png", 22, 30, 28, baseDate);
                AddProduct(products, ref productId, "Mixed Kebab Platter", 7, 120, 32, "Assorted meat kebabs with grilled vegetables", "/images/g8.png", 22, 28, 32, baseDate);
                AddProduct(products, ref productId, "Chicken Drumsticks", 7, 65, 60, "Grilled chicken drumsticks with spices", "/images/g9.png", 18, 25, 60, baseDate);
                AddProduct(products, ref productId, "BBQ Wings Special", 7, 85, 45, "Sticky BBQ chicken wings with sauce", "/images/g111.png", 20, 25, 45, baseDate);

                // PIZZA
                AddProduct(products, ref productId, "Margherita Pizza", 8, 65, 50, "Classic pizza with tomato, mozzarella, and basil", "/images/p1.png", 15, 22, 50, baseDate);
                AddProduct(products, ref productId, "Pepperoni Pizza", 8, 75, 55, "Classic pepperoni with mozzarella cheese", "/images/p2.png", 15, 22, 55, baseDate);
                AddProduct(products, ref productId, "Supreme Pizza", 8, 95, 45, "Loaded with pepperoni, sausage, peppers, and more", "/images/p3.png", 18, 25, 45, baseDate);
                AddProduct(products, ref productId, "Vegetarian Pizza", 8, 70, 48, "Fresh vegetables with mozzarella and tomato sauce", "/images/p4.png", 15, 22, 48, baseDate);
                AddProduct(products, ref productId, "Cheese Lovers Pizza", 8, 80, 42, "Four cheese blend pizza", "/images/p5.png", 16, 23, 42, baseDate);
                AddProduct(products, ref productId, "BBQ Chicken Pizza", 8, 85, 40, "Grilled chicken with BBQ sauce and onions", "/images/p6.png", 18, 25, 40, baseDate);
                AddProduct(products, ref productId, "Hawaiian Pizza", 8, 75, 46, "Ham and pineapple with mozzarella", "/images/p7.png", 15, 22, 46, baseDate);
                AddProduct(products, ref productId, "Meat Feast Pizza", 8, 98, 38, "Loaded with various meats and cheese", "/images/p8.png", 18, 26, 38, baseDate);

                // PIES & PASTRIES
                AddProduct(products, ref productId, "Apple Pie", 9, 55, 45, "Classic American apple pie with cinnamon", "/images/s1.png", 10, 15, 45, baseDate);
                AddProduct(products, ref productId, "Cherry Pie", 9, 58, 40, "Sweet cherry pie with lattice crust", "/images/s2.png", 10, 15, 40, baseDate);
                AddProduct(products, ref productId, "Crepes", 9, 42, 60, "French crepes with sweet filling", "/images/s3.png", 8, 12, 60, baseDate);
                AddProduct(products, ref productId, "Strawberry Waffle", 9, 48, 55, "Belgian waffle topped with fresh strawberries", "/images/s4.png", 10, 15, 55, baseDate);
                AddProduct(products, ref productId, "Butter Croissants", 9, 25, 80, "Flaky French butter croissants", "/images/s5.png", 5, 8, 80, baseDate);
                AddProduct(products, ref productId, "Cinnamon Roll", 9, 32, 70, "Sweet cinnamon roll with cream cheese frosting", "/images/s6.png", 8, 12, 70, baseDate);
                AddProduct(products, ref productId, "Mixed Pastry Box", 9, 65, 35, "Assorted sweet pastries and cookies", "/images/s7.png", 10, 12, 35, baseDate);
                AddProduct(products, ref productId, "Danish Pastries", 9, 38, 65, "Assorted Danish pastries with fruit filling", "/images/s8.png", 8, 10, 65, baseDate);
                AddProduct(products, ref productId, "Breakfast Sandwich", 9, 45, 60, "Fresh breakfast sandwich with egg and cheese", "/images/s9.png", 10, 15, 60, baseDate);

                builder.HasData(products);
            }

            private void AddProduct(List<MenuProduct> products, ref int productId, string name, int categoryId, decimal price, int quantity, string description, string imageUrl, int minTime, int maxTime, int dayStock, DateTime createdAt)
            {
                products.Add(new MenuProduct
                {
                    Id = productId++,
                    Name = name,
                    CategoryId = categoryId,
                    Price = price,
                    Quantity = quantity,
                    Description = description,
                    ImageUrl = imageUrl,
                    MinTime = minTime,
                    MaxTime = maxTime,
                    DayStock = dayStock,
                    CreatedAt = createdAt
                });
            }
        }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var hasher = new PasswordHasher<User>();

            var users = new[]
            {
                new User
                {
                    Id = "1",
                    UserName = "medo03459",
                    NormalizedUserName = "MEDO03459",
                    Email = "medo03459@gmail.com",
                    NormalizedEmail = "MEDO03459@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "asd123"),
                    PhoneNumber = "+201123002663",
                    Birthday = new DateTime(1999, 11, 19),
                    IsAdmin = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new User
                {
                    Id = "2",
                    UserName = "4dm1n",
                    NormalizedUserName = "4DM1N",
                    Email = "4dm1n@gmail.com",
                    NormalizedEmail = "4DM1N@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "asd123"),
                    PhoneNumber = "+201123002663",
                    Birthday = new DateTime(1999, 11, 19),
                    IsAdmin = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            };

            builder.HasData(users);
        }
    }


}
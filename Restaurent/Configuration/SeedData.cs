using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurent.Models;
using System.Security.Cryptography;
using System.Text;

namespace Restaurent.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { Id = 1, Name = "Salads", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 2, Name = "Soups", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 3, Name = "Burgers", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 4, Name = "Cakes & Desserts", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 5, Name = "Beverages", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 6, Name = "Seafood", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 7, Name = "Grilled & BBQ", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 8, Name = "Pizza", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 9, Name = "Pies & Pastries", CreatedAt = new DateTime(2024, 1, 1) }
            );
        }
    }

    public class MenuProductConfiguration : IEntityTypeConfiguration<MenuProduct>
    {
        public void Configure(EntityTypeBuilder<MenuProduct> builder)
        {
            var baseDate = new DateTime(2024, 1, 1);
            var products = new List<MenuProduct>();
            int productId = 1;

            // ==================== SALADS ====================
            products.Add(new MenuProduct { Id = productId++, Name = "Fresh Garden Salad", CategoryId = 1, Price = 45, Quantity = 50, Description = "Fresh mixed greens with cherry tomatoes, cucumbers, and house dressing", ImageUrl = "/images/a1.png", MinTime = 5, MaxTime = 10, DayStock = 50, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Fruit Paradise Bowl", CategoryId = 1, Price = 55, Quantity = 40, Description = "Colorful assorted fresh fruits with honey drizzle", ImageUrl = "/images/a2.png", MinTime = 5, MaxTime = 8, DayStock = 40, CreatedAt = baseDate });

            // ==================== SOUPS ====================
            products.Add(new MenuProduct { Id = productId++, Name = "Creamy Corn Soup", CategoryId = 2, Price = 35, Quantity = 60, Description = "Rich and creamy sweet corn soup", ImageUrl = "/images/a3.png", MinTime = 8, MaxTime = 12, DayStock = 60, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Classic Tomato Soup", CategoryId = 2, Price = 30, Quantity = 55, Description = "Traditional tomato soup with fresh basil", ImageUrl = "/images/a4.png", MinTime = 8, MaxTime = 12, DayStock = 55, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Creamy Mushroom Soup", CategoryId = 2, Price = 40, Quantity = 50, Description = "Velvety mushroom soup with herbs", ImageUrl = "/images/a5.png", MinTime = 10, MaxTime = 15, DayStock = 50, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Seafood Chowder", CategoryId = 2, Price = 65, Quantity = 35, Description = "Rich seafood soup with prawns and fish", ImageUrl = "/images/a6.png", MinTime = 12, MaxTime = 18, DayStock = 35, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Golden Egg Drop Soup", CategoryId = 2, Price = 28, Quantity = 60, Description = "Light and fluffy egg drop soup", ImageUrl = "/images/a7.png", MinTime = 8, MaxTime = 10, DayStock = 60, CreatedAt = baseDate });

            // ==================== BURGERS & SIDES ====================
            products.Add(new MenuProduct { Id = productId++, Name = "Golden French Fries", CategoryId = 3, Price = 25, Quantity = 100, Description = "Crispy golden French fries", ImageUrl = "/images/a8.png", MinTime = 8, MaxTime = 12, DayStock = 100, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Ketchup Dip", CategoryId = 3, Price = 8, Quantity = 200, Description = "Classic tomato ketchup", ImageUrl = "/images/a9.png", MinTime = 1, MaxTime = 2, DayStock = 200, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Classic Beef Burger", CategoryId = 3, Price = 55, Quantity = 80, Description = "Juicy beef patty with cheese and fresh vegetables", ImageUrl = "/images/b1.png", MinTime = 15, MaxTime = 20, DayStock = 80, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Double Decker Burger", CategoryId = 3, Price = 75, Quantity = 60, Description = "Two beef patties with double cheese", ImageUrl = "/images/b2.png", MinTime = 18, MaxTime = 25, DayStock = 60, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Triple Stack Burger", CategoryId = 3, Price = 95, Quantity = 45, Description = "Three layers of beef patties with special sauce", ImageUrl = "/images/b3.png", MinTime = 20, MaxTime = 28, DayStock = 45, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Mega Monster Burger", CategoryId = 3, Price = 110, Quantity = 30, Description = "Huge burger with multiple patties and toppings", ImageUrl = "/images/b4.png", MinTime = 25, MaxTime = 30, DayStock = 30, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Deluxe Combo Burger", CategoryId = 3, Price = 65, Quantity = 70, Description = "Burger combo with fries", ImageUrl = "/images/b5.png", MinTime = 18, MaxTime = 22, DayStock = 70, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Crispy Chicken Burger", CategoryId = 3, Price = 50, Quantity = 75, Description = "Crispy fried chicken with fresh lettuce", ImageUrl = "/images/b6.png", MinTime = 15, MaxTime = 20, DayStock = 75, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "BBQ Bacon Burger", CategoryId = 3, Price = 70, Quantity = 55, Description = "Beef burger with crispy bacon and BBQ sauce", ImageUrl = "/images/b7.png", MinTime = 18, MaxTime = 23, DayStock = 55, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Premium Gourmet Burger", CategoryId = 3, Price = 85, Quantity = 40, Description = "Premium beef with caramelized onions and special sauce", ImageUrl = "/images/b8.png", MinTime = 20, MaxTime = 25, DayStock = 40, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Spicy Jalapeño Burger", CategoryId = 3, Price = 60, Quantity = 65, Description = "Spicy burger with jalapeños and pepper jack cheese", ImageUrl = "/images/b9.png", MinTime = 16, MaxTime = 22, DayStock = 65, CreatedAt = baseDate });

            // ==================== CAKES & DESSERTS ====================
            products.Add(new MenuProduct { Id = productId++, Name = "Vanilla Cupcake", CategoryId = 4, Price = 20, Quantity = 100, Description = "Soft vanilla cupcake with cream frosting", ImageUrl = "/images/c1.png", MinTime = 5, MaxTime = 8, DayStock = 100, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Chocolate Cupcake", CategoryId = 4, Price = 22, Quantity = 95, Description = "Rich chocolate cupcake with chocolate frosting", ImageUrl = "/images/c2.png", MinTime = 5, MaxTime = 8, DayStock = 95, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Passion Fruit Tart", CategoryId = 4, Price = 35, Quantity = 50, Description = "Tangy passion fruit tart with fresh cream", ImageUrl = "/images/c3.png", MinTime = 8, MaxTime = 10, DayStock = 50, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Black Forest Cake", CategoryId = 4, Price = 120, Quantity = 20, Description = "Classic Black Forest cake with cherries and whipped cream", ImageUrl = "/images/c4.png", MinTime = 10, MaxTime = 15, DayStock = 20, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Strawberry Cheesecake", CategoryId = 4, Price = 65, Quantity = 40, Description = "Creamy cheesecake topped with fresh strawberries", ImageUrl = "/images/c5.png", MinTime = 8, MaxTime = 12, DayStock = 40, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Cookies & Cream Cake", CategoryId = 4, Price = 55, Quantity = 45, Description = "Chocolate cake with Oreo cream filling", ImageUrl = "/images/c6.png", MinTime = 8, MaxTime = 12, DayStock = 45, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Red Velvet Cake Slice", CategoryId = 4, Price = 45, Quantity = 60, Description = "Classic red velvet cake with cream cheese frosting", ImageUrl = "/images/c7.png", MinTime = 6, MaxTime = 10, DayStock = 60, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Lemon Meringue Pie", CategoryId = 4, Price = 40, Quantity = 35, Description = "Tangy lemon filling with fluffy meringue topping", ImageUrl = "/images/c8.png", MinTime = 8, MaxTime = 10, DayStock = 35, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Ice Cream Sundae", CategoryId = 4, Price = 38, Quantity = 80, Description = "Chocolate and vanilla ice cream with toppings", ImageUrl = "/images/c9.png", MinTime = 5, MaxTime = 8, DayStock = 80, CreatedAt = baseDate });

            // ==================== BEVERAGES ====================
            products.Add(new MenuProduct { Id = productId++, Name = "Green Detox Smoothie", CategoryId = 5, Price = 32, Quantity = 60, Description = "Healthy green smoothie with spinach and fruits", ImageUrl = "/images/d2.png", MinTime = 5, MaxTime = 8, DayStock = 60, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Mint Mojito", CategoryId = 5, Price = 28, Quantity = 70, Description = "Refreshing mint mojito with lime", ImageUrl = "/images/d3.png", MinTime = 5, MaxTime = 8, DayStock = 70, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Fresh Orange Juice", CategoryId = 5, Price = 25, Quantity = 80, Description = "Freshly squeezed orange juice", ImageUrl = "/images/d4.png", MinTime = 5, MaxTime = 7, DayStock = 80, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Strawberry Milkshake", CategoryId = 5, Price = 35, Quantity = 65, Description = "Creamy strawberry milkshake", ImageUrl = "/images/d5.png", MinTime = 6, MaxTime = 10, DayStock = 65, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Watermelon Juice", CategoryId = 5, Price = 22, Quantity = 75, Description = "Fresh watermelon juice", ImageUrl = "/images/d6.png", MinTime = 5, MaxTime = 8, DayStock = 75, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Lemonade", CategoryId = 5, Price = 20, Quantity = 90, Description = "Classic fresh lemonade", ImageUrl = "/images/d7.png", MinTime = 5, MaxTime = 7, DayStock = 90, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Strawberry Smoothie", CategoryId = 5, Price = 30, Quantity = 70, Description = "Thick strawberry smoothie", ImageUrl = "/images/d8.png", MinTime = 6, MaxTime = 9, DayStock = 70, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Mango Smoothie", CategoryId = 5, Price = 32, Quantity = 65, Description = "Tropical mango smoothie", ImageUrl = "/images/d9.png", MinTime = 6, MaxTime = 9, DayStock = 65, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Hot Tea", CategoryId = 5, Price = 15, Quantity = 100, Description = "Traditional hot tea", ImageUrl = "/images/d10.png", MinTime = 5, MaxTime = 8, DayStock = 100, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Cappuccino", CategoryId = 5, Price = 28, Quantity = 85, Description = "Rich Italian cappuccino", ImageUrl = "/images/d11.png", MinTime = 6, MaxTime = 10, DayStock = 85, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Latte Art Coffee", CategoryId = 5, Price = 30, Quantity = 75, Description = "Artistic latte with milk foam", ImageUrl = "/images/d12.png", MinTime = 7, MaxTime = 12, DayStock = 75, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Espresso", CategoryId = 5, Price = 25, Quantity = 90, Description = "Strong Italian espresso", ImageUrl = "/images/d13.png", MinTime = 5, MaxTime = 8, DayStock = 90, CreatedAt = baseDate });

            // ==================== SEAFOOD ====================
            products.Add(new MenuProduct { Id = productId++, Name = "Shrimp Platter", CategoryId = 6, Price = 95, Quantity = 40, Description = "Fresh shrimp with cocktail sauce", ImageUrl = "/images/f1.png", MinTime = 15, MaxTime = 20, DayStock = 40, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Sushi Platter", CategoryId = 6, Price = 120, Quantity = 35, Description = "Assorted fresh sushi rolls", ImageUrl = "/images/f2.png", MinTime = 20, MaxTime = 25, DayStock = 35, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Grilled Fish Fillet", CategoryId = 6, Price = 85, Quantity = 45, Description = "Perfectly grilled fish with lemon", ImageUrl = "/images/f3.png", MinTime = 18, MaxTime = 25, DayStock = 45, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Grilled Salmon", CategoryId = 6, Price = 110, Quantity = 30, Description = "Premium grilled salmon steak", ImageUrl = "/images/f4.png", MinTime = 20, MaxTime = 28, DayStock = 30, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Calamari Rings", CategoryId = 6, Price = 65, Quantity = 55, Description = "Crispy fried calamari rings", ImageUrl = "/images/f5.png", MinTime = 12, MaxTime = 18, DayStock = 55, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Fish & Chips", CategoryId = 6, Price = 70, Quantity = 50, Description = "Classic British fish and chips", ImageUrl = "/images/f6.png", MinTime = 15, MaxTime = 22, DayStock = 50, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Breaded Shrimp", CategoryId = 6, Price = 75, Quantity = 48, Description = "Crispy breaded jumbo shrimp", ImageUrl = "/images/f7.png", MinTime = 15, MaxTime = 20, DayStock = 48, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Seafood Paella", CategoryId = 6, Price = 130, Quantity = 25, Description = "Spanish seafood rice with mixed seafood", ImageUrl = "/images/f8.png", MinTime = 25, MaxTime = 35, DayStock = 25, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Lobster Special", CategoryId = 6, Price = 180, Quantity = 15, Description = "Grilled whole lobster with butter sauce", ImageUrl = "/images/f9.png", MinTime = 30, MaxTime = 40, DayStock = 15, CreatedAt = baseDate });

            // ==================== GRILLED & BBQ ====================
            products.Add(new MenuProduct { Id = productId++, Name = "Spicy Wings", CategoryId = 7, Price = 55, Quantity = 70, Description = "Hot and spicy chicken wings", ImageUrl = "/images/g1.png", MinTime = 15, MaxTime = 20, DayStock = 70, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "BBQ Chicken Wings", CategoryId = 7, Price = 58, Quantity = 65, Description = "Chicken wings with BBQ glaze", ImageUrl = "/images/g2.png", MinTime = 15, MaxTime = 22, DayStock = 65, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Mixed Grill Platter", CategoryId = 7, Price = 140, Quantity = 35, Description = "Assorted grilled meats with vegetables", ImageUrl = "/images/g3.png", MinTime = 25, MaxTime = 35, DayStock = 35, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Grilled Steak", CategoryId = 7, Price = 150, Quantity = 30, Description = "Premium beef steak grilled to perfection", ImageUrl = "/images/g4.png", MinTime = 22, MaxTime = 30, DayStock = 30, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Chicken Kebab Skewers", CategoryId = 7, Price = 75, Quantity = 55, Description = "Grilled chicken kebabs with spices", ImageUrl = "/images/g5.png", MinTime = 18, MaxTime = 25, DayStock = 55, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "BBQ Ribs", CategoryId = 7, Price = 95, Quantity = 40, Description = "Tender BBQ pork ribs", ImageUrl = "/images/g6.png", MinTime = 25, MaxTime = 35, DayStock = 40, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Grilled Lamb Chops", CategoryId = 7, Price = 135, Quantity = 28, Description = "Juicy grilled lamb chops with herbs", ImageUrl = "/images/g7.png", MinTime = 22, MaxTime = 30, DayStock = 28, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Mixed Kebab Platter", CategoryId = 7, Price = 120, Quantity = 32, Description = "Assorted meat kebabs with grilled vegetables", ImageUrl = "/images/g8.png", MinTime = 22, MaxTime = 28, DayStock = 32, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Chicken Drumsticks", CategoryId = 7, Price = 65, Quantity = 60, Description = "Grilled chicken drumsticks with spices", ImageUrl = "/images/g9.png", MinTime = 18, MaxTime = 25, DayStock = 60, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "BBQ Wings Special", CategoryId = 7, Price = 85, Quantity = 45, Description = "Sticky BBQ chicken wings with sauce", ImageUrl = "/images/g111.png", MinTime = 20, MaxTime = 25, DayStock = 45, CreatedAt = baseDate });

            // ==================== PIZZA ====================
            products.Add(new MenuProduct { Id = productId++, Name = "Margherita Pizza", CategoryId = 8, Price = 65, Quantity = 50, Description = "Classic pizza with tomato, mozzarella, and basil", ImageUrl = "/images/p1.png", MinTime = 15, MaxTime = 22, DayStock = 50, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Pepperoni Pizza", CategoryId = 8, Price = 75, Quantity = 55, Description = "Classic pepperoni with mozzarella cheese", ImageUrl = "/images/p2.png", MinTime = 15, MaxTime = 22, DayStock = 55, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Supreme Pizza", CategoryId = 8, Price = 95, Quantity = 45, Description = "Loaded with pepperoni, sausage, peppers, and more", ImageUrl = "/images/p3.png", MinTime = 18, MaxTime = 25, DayStock = 45, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Vegetarian Pizza", CategoryId = 8, Price = 70, Quantity = 48, Description = "Fresh vegetables with mozzarella and tomato sauce", ImageUrl = "/images/p4.png", MinTime = 15, MaxTime = 22, DayStock = 48, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Cheese Lovers Pizza", CategoryId = 8, Price = 80, Quantity = 42, Description = "Four cheese blend pizza", ImageUrl = "/images/p5.png", MinTime = 16, MaxTime = 23, DayStock = 42, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "BBQ Chicken Pizza", CategoryId = 8, Price = 85, Quantity = 40, Description = "Grilled chicken with BBQ sauce and onions", ImageUrl = "/images/p6.png", MinTime = 18, MaxTime = 25, DayStock = 40, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Hawaiian Pizza", CategoryId = 8, Price = 75, Quantity = 46, Description = "Ham and pineapple with mozzarella", ImageUrl = "/images/p7.png", MinTime = 15, MaxTime = 22, DayStock = 46, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Meat Feast Pizza", CategoryId = 8, Price = 98, Quantity = 38, Description = "Loaded with various meats and cheese", ImageUrl = "/images/p8.png", MinTime = 18, MaxTime = 26, DayStock = 38, CreatedAt = baseDate });

            // ==================== PIES & PASTRIES ====================
            products.Add(new MenuProduct { Id = productId++, Name = "Apple Pie", CategoryId = 9, Price = 55, Quantity = 45, Description = "Classic American apple pie with cinnamon", ImageUrl = "/images/s1.png", MinTime = 10, MaxTime = 15, DayStock = 45, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Cherry Pie", CategoryId = 9, Price = 58, Quantity = 40, Description = "Sweet cherry pie with lattice crust", ImageUrl = "/images/s2.png", MinTime = 10, MaxTime = 15, DayStock = 40, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Crepes", CategoryId = 9, Price = 42, Quantity = 60, Description = "French crepes with sweet filling", ImageUrl = "/images/s3.png", MinTime = 8, MaxTime = 12, DayStock = 60, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Strawberry Waffle", CategoryId = 9, Price = 48, Quantity = 55, Description = "Belgian waffle topped with fresh strawberries", ImageUrl = "/images/s4.png", MinTime = 10, MaxTime = 15, DayStock = 55, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Butter Croissants", CategoryId = 9, Price = 25, Quantity = 80, Description = "Flaky French butter croissants", ImageUrl = "/images/s5.png", MinTime = 5, MaxTime = 8, DayStock = 80, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Cinnamon Roll", CategoryId = 9, Price = 32, Quantity = 70, Description = "Sweet cinnamon roll with cream cheese frosting", ImageUrl = "/images/s6.png", MinTime = 8, MaxTime = 12, DayStock = 70, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Mixed Pastry Box", CategoryId = 9, Price = 65, Quantity = 35, Description = "Assorted sweet pastries and cookies", ImageUrl = "/images/s7.png", MinTime = 10, MaxTime = 12, DayStock = 35, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Danish Pastries", CategoryId = 9, Price = 38, Quantity = 65, Description = "Assorted Danish pastries with fruit filling", ImageUrl = "/images/s8.png", MinTime = 8, MaxTime = 10, DayStock = 65, CreatedAt = baseDate });
            products.Add(new MenuProduct { Id = productId++, Name = "Breakfast Sandwich", CategoryId = 9, Price = 45, Quantity = 60, Description = "Fresh breakfast sandwich with egg and cheese", ImageUrl = "/images/s9.png", MinTime = 10, MaxTime = 15, DayStock = 60, CreatedAt = baseDate });

            builder.HasData(products);
        }
    }
    
        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                // دالة لتشفير كلمة المرور
                string HashPassword(string password)
                {
                    using var sha256 = SHA256.Create();
                    var bytes = Encoding.UTF8.GetBytes(password);
                    var hash = sha256.ComputeHash(bytes);
                    return Convert.ToBase64String(hash);
                }


                builder.HasData(
                    new User
                    {
                        Id = 1,
                        Name = "Super Admin",
                        Email = "medo03459@gmail.com",
                        Password = HashPassword("I*L0ve*ME"),
                        Phone = "+201123002663",
                        Birthday = new DateTime(1999, 11, 19),
                        IsAdmin = true,
                        CreatedAt = new DateTime(2024, 1, 1)
                    }
                );
            }
        }
    
}
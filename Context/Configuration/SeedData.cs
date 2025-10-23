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
                new Category { Id = 1, Name = "الكل", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 2, Name = "الحلويات", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 3, Name = "المشروبات", CreatedAt = new DateTime(2024, 1, 1) }
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

            // الحلويات
            AddProduct(products, ref productId, "سوشي كريب", 2, 24.00m, 50, "Sushi crepe Cal 678", "/images/9.jpg", 10, 15, 50, baseDate);
            AddProduct(products, ref productId, "مولتن شوكلاتة", 2, 23.00m, 45, "Molten chocolate Cal 420", "/images/12.jpg", 12, 18, 45, baseDate);
            AddProduct(products, ref productId, "وافل بالزعتر", 2, 23.00m, 55, "Waffle zatar Cal 620", "/images/5.jpg", 8, 12, 55, baseDate);
            AddProduct(products, ref productId, "وافل كبير", 2, 21.00m, 60, "Big waffle Cal 620", "/images/4.jpg", 8, 12, 60, baseDate);
            AddProduct(products, ref productId, "اصابع الوافل", 2, 21.00m, 65, "Waffle fingers Cal 620", "/images/6.jpg", 10, 15, 65, baseDate);
            AddProduct(products, ref productId, "بانكيك", 2, 16.00m, 70, "Pancake Cal 569", "/images/2.jpg", 8, 12, 70, baseDate);
            AddProduct(products, ref productId, "وافل صغير", 2, 16.00m, 75, "Small waffle Cal 569", "/images/1.jpg", 6, 10, 75, baseDate);
            AddProduct(products, ref productId, "مكس بوكس", 2, 21.00m, 40, "Mix box Cal 853", "/images/7.jpg", 12, 18, 40, baseDate);
            AddProduct(products, ref productId, "ستيك فراولة", 2, 19.00m, 55, "Stik strawberry Cal 248", "/images/20.jpg", 10, 15, 55, baseDate);
            AddProduct(products, ref productId, "كيك تمر", 2, 23.00m, 35, "Keek tamer Cal 450", "/images/19.jpg", 15, 20, 35, baseDate);
            AddProduct(products, ref productId, "مولتن وايت شوكلاتة", 2, 24.00m, 40, "Molten White Cal 420", "/images/13.jpg", 12, 18, 40, baseDate);
            AddProduct(products, ref productId, "بوكس بانكيك", 2, 52.00m, 25, "Box pancake Cal 1950", "/images/8.jpg", 18, 25, 25, baseDate);
            AddProduct(products, ref productId, "كريب", 2, 20.00m, 60, "Crepe Cal 620", "/images/10.jpg", 8, 12, 60, baseDate);
            AddProduct(products, ref productId, "كريب فراولة", 2, 25.00m, 45, "Strawberry crepe Cal 640", "/images/11.jpg", 10, 15, 45, baseDate);
            AddProduct(products, ref productId, "حقنة الشوكلاتة", 2, 12.00m, 80, "Chocolate needle Cal 41", "/images/17.jpg", 5, 8, 80, baseDate);
            AddProduct(products, ref productId, "كاندريلا كريب", 2, 24.00m, 35, "Canderella crepe Cal 720", "/images/18.jpg", 12, 18, 35, baseDate);

            // المشروبات
            AddProduct(products, ref productId, "قهوة اليوم بارده", 3, 9.00m, 100, "Cal 8", "/images/15.jpg", 3, 5, 100, baseDate);
            AddProduct(products, ref productId, "قهوة اليوم حارة", 3, 8.00m, 120, "Cal 4", "/images/16.jpg", 3, 5, 120, baseDate);
            AddProduct(products, ref productId, "ماء", 3, 1.00m, 200, "Water", "/images/21.jpg", 1, 2, 200, baseDate);

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
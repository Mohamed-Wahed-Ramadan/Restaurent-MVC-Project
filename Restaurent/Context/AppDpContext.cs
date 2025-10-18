using Microsoft.EntityFrameworkCore;
using Restaurent.Configuration;
using Restaurent.Models;

namespace Restaurent.Context
{
    public class AppDpContext : DbContext
    {
        // أضف هذا الكونستركتور
        //public AppDpContext(DbContextOptions<AppDpContext> options) : base(options)
        //{
        //}

        // احتفظ بالكونستركتور القديم لأغراض التوافق
        public AppDpContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Connection.DataSource);
            }
        }

        // DbSets الحالية
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }
        public DbSet<User> Users { get; set; }

        // DbSets الجديدة
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // تطبيق التهيئة للنماذج الحالية
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MenuProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            // تكوين العلاقات الجديدة
            ConfigureRelationships(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // تكوين العلاقة بين User و Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.MenuProduct)
                .WithMany()
                .HasForeignKey(c => c.MenuProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // تكوين العلاقة بين User و Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // تكوين العلاقة بين Order و OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuProduct)
                .WithMany()
                .HasForeignKey(oi => oi.MenuProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // تكوين العلاقة بين User و Favorite
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.MenuProduct)
                .WithMany()
                .HasForeignKey(f => f.MenuProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // تكوين العلاقة بين Discount و Category
            modelBuilder.Entity<Discount>()
                .HasOne(d => d.Category)
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // ضمان أن UniqueOrderId فريد
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.UniqueOrderId)
                .IsUnique();

            // ضمان أن مجموعة (UserId, MenuProductId) فريدة في Cart
            modelBuilder.Entity<Cart>()
                .HasIndex(c => new { c.UserId, c.MenuProductId })
                .IsUnique();

            // ضمان أن مجموعة (UserId, MenuProductId) فريدة في Favorite
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.MenuProductId })
                .IsUnique();
        }
    }
}
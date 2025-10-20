using Context.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Context
{
    public class AppDpContext : IdentityDbContext<User>
    {
        public AppDpContext(DbContextOptions<AppDpContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تطبيق التهيئة للنماذج
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MenuProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            // تكوين العلاقات والفهارس
            ConfigureRelationships(modelBuilder);
            ConfigureIndexes(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Cart Relationships
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

            // Order Relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem Relationships
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

            // Favorite Relationships
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

            // Discount Relationships
            modelBuilder.Entity<Discount>()
                .HasOne(d => d.Category)
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // MenuProduct Relationships
            modelBuilder.Entity<MenuProduct>()
                .HasOne(mp => mp.Category)
                .WithMany(c => c.MenuProduct)
                .HasForeignKey(mp => mp.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Unique Indexes
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.UniqueOrderId)
                .IsUnique();

            modelBuilder.Entity<Cart>()
                .HasIndex(c => new { c.UserId, c.MenuProductId })
                .IsUnique();

            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.MenuProductId })
                .IsUnique();

            // Performance Indexes
            modelBuilder.Entity<MenuProduct>()
                .HasIndex(mp => mp.Name);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.Status);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.CreatedAt);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Restaurent.Configuration;
using Restaurent.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Restaurent.Context
{
    public class AppDpContext : DbContext
    {
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Connection.DataSource);

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MenuProductConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<BaseModel>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = null;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;
using SyriaTrustPlanning.Domain.Entities.CategoryProductModel;
using SyriaTrustPlanning.Domain.Entities.IdentityModels;
using SyriaTrustPlanning.Domain.Entities.ProductModel;

namespace SyriaTrustPlanning.Persistence
{
    public class SyriaTrustPlanningDbContext : DbContext
    {
        public SyriaTrustPlanningDbContext(DbContextOptions<SyriaTrustPlanningDbContext> options)
            :base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasQueryFilter(p => !p.isDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.isDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(p => !p.isDeleted);
            modelBuilder.Entity<CategoryProduct>().HasQueryFilter(p => !p.isDeleted);

            modelBuilder.Entity<User>()
                .HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}

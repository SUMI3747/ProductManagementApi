using Microsoft.EntityFrameworkCore;
using PrdouctsApi.Models.Entities;

namespace PrdouctsApi.Data
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>().HasKey(e => e.ProductId);
            modelBuilder.Entity<Products>().Property(e => e.ProductId).ValueGeneratedOnAdd().UseIdentityColumn(100000, 1);
            modelBuilder.Entity<Products>().ToTable("tbl_Products");
        }
    }
}

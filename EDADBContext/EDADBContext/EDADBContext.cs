using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EDADBContext.Models;

namespace EDADBContext
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

       
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Config> Config { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }

    public class AppDbContextFactory : IDesignTimeDbContextFactory<DBContext>
    {
        public DBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DBContext>();
            optionsBuilder.UseSqlServer("Server=COGNINE-L219;Initial Catalog=EDA;Integrated Security=True;TrustServerCertificate=True");

            return new DBContext(optionsBuilder.Options);
        }
    }
}

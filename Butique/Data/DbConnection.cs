using Microsoft.EntityFrameworkCore;
using Butique.Models.Entity;

namespace Butique.Data
{
    public class DbConnection: DbContext
    {
        public DbConnection(DbContextOptions op) : base(op) { }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}

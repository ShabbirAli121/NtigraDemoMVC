using Microsoft.EntityFrameworkCore;
using NtigraDemoMVC.Models.Domain;

namespace NtigraDemoMVC.Data
{
    public class NtigraDemoDBContext : DbContext
    {
        public NtigraDemoDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Taxio.DataAccess.Models;

namespace Taxio.DataAccess
{
    public class TaxioDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; } = null!;

        public TaxioDbContext(DbContextOptions<TaxioDbContext> options) : base(options)
        { }
    }
}

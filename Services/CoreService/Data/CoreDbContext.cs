using CoreService.model;
using Microsoft.EntityFrameworkCore;

namespace CoreService.Data
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> employees { get; set; }

    }
}

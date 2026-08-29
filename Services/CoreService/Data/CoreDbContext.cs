using CoreService.model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CoreService.Data
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasKey(e => e.Id);

            modelBuilder.Entity<Department>().HasKey(d => d.Id);

            modelBuilder.Entity<Employee>().HasOne(d => d.Department)
                .WithMany(x => x.Employees).HasForeignKey(d => d.DepartmentId).OnDelete(DeleteBehavior.Restrict);

        }

        
    }

   
}

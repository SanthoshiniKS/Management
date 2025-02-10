using Microsoft.EntityFrameworkCore;
using Management.Models;

namespace Management.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<City> Cities { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<Department>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Seed();

            modelBuilder.Entity<Employee>().HasOne(e => e.Department).WithMany().HasForeignKey(e => e.DepartmentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Employee>().HasOne(e => e.City).WithMany().HasForeignKey(e => e.CityId).OnDelete(DeleteBehavior.Restrict);
        }
       
    }
}

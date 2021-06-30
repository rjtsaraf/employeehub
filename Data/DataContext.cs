using EmpDepAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpDepAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Employee> Employees{get;set;}
        public DbSet<Department> Departments{get;set;}
        public DbSet<Project> Projects{get;set;}

    }
}